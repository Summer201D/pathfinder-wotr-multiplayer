using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Microsoft.Extensions.Logging;
using static Kingmaker.RuleSystem.RulebookEvent;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleCalculateDamagePatches
    {
        [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.OnTrigger))]
        [HarmonyPostfix]
        public static void RuleCalculateDamage_OnTrigger_Postfix(RuleCalculateDamage __instance)
        {
            if (!Main.Multiplayer.IsActive || PatchesUtils.IsHelperUnit(__instance.Target.UniqueId))
            {
                return;
            }

            Main.Rolls.OnAfterRuleCalculateDamageBundle(__instance);
        }

        [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.OnTrigger))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleCalculateDamage_OnTrigger_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RuleCalculateDamagePatches), nameof(RuleCalculateDamagePatches.OnCalculateDamageBundle));
            var lookFor = AccessTools.Field(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.CalculatedDamage));
            var match = matcher.SearchForward(x => x.LoadsField(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleCalculateDamagePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith)
            };
            match = match.Advance(-1).RemoveInstructions(10).Insert(newInstructions);
            Main.GetLogger<RuleCalculateDamagePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.Roll))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleCalculateDamage_Roll_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!ReplaceNonTacticalCombat(matcher, target) || !ReplaceTacticalCombat(matcher, target))
            {
                Main.GetLogger<RuleCalculateDamagePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            return matcher.Instructions();
        }

        private static bool ReplaceNonTacticalCombat(CodeMatcher matcher, string target)
        {
            var lookFor = AccessTools.Method(typeof(Dice), nameof(Dice.D), [typeof(DiceFormula)]);
            var replaceWith = AccessTools.Method(typeof(RuleCalculateDamagePatches), nameof(RuleCalculateDamagePatches.OnCalculateDamageRoll));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleCalculateDamagePatches>().LogError("Transpiler has not been applied (ReplaceNonTacticalCombat). Target={Target}", target);
                return false;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstruction().Insert(newInstructions);
            Main.GetLogger<RuleCalculateDamagePatches>().LogDebug("Transpiler has been applied (ReplaceNonTacticalCombat). Target={Target}", target);
            return true;
        }

        private static bool ReplaceTacticalCombat(CodeMatcher matcher, string target)
        {
            var lookFor = AccessTools.Method(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.GetTacticalCombatRoll));
            var replaceWith = AccessTools.Method(typeof(RuleCalculateDamagePatches), nameof(RuleCalculateDamagePatches.OnCalculateDamageRoll));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleCalculateDamagePatches>().LogError("Transpiler has not been applied (ReplaceTacticalCombat). Target={Target}", target);
                return false;
            }

            match = match.Advance(-3);
            var labels = match.Instruction.ExtractLabels();
            var newInstructions = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_1).WithLabels(labels),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldc_I4_1),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstructions(4).Insert(newInstructions);

            Main.GetLogger<RuleCalculateDamagePatches>().LogDebug("Transpiler has been applied (ReplaceTacticalCombat). Target={Target}", target);
            return true;
        }

        private static void OnCalculateDamageBundle(RuleCalculateDamage ruleCalculateDamage)
        {
            if (Main.Multiplayer.IsActive && !PatchesUtils.IsHelperUnit(ruleCalculateDamage.Target.UniqueId))
            {
                var shouldRunOriginalLogic = Main.Rolls.OnBeforeRuleCalculateDamageBundle(ruleCalculateDamage);
                if (!shouldRunOriginalLogic)
                {
                    return;
                }
            }

            ruleCalculateDamage.CalculatedDamage.InsertRange(0, ruleCalculateDamage.DamageBundle.Select(ruleCalculateDamage.CalculateDamageValue));
        }

        private static int OnCalculateDamageRoll(DiceFormula diceFormula, int unitsCount, bool isTacticalCombat, RuleCalculateDamage ruleCalculateDamage)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return isTacticalCombat ? TacticalCombatHelper.GetDiceResult(diceFormula) : Dice.D(diceFormula);
            }

            int? roll = Main.Rolls.OnBeforeRuleCalculateDamageRoll(ruleCalculateDamage, diceFormula);
            if (roll.HasValue)
            {
                return roll.Value;
            }

            return isTacticalCombat ? ruleCalculateDamage.GetTacticalCombatRoll(diceFormula, unitsCount) : Dice.D(diceFormula);
        }
    }
}
