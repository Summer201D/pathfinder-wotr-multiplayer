using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Microsoft.Extensions.Logging;
using static Kingmaker.RuleSystem.RulebookEvent;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleCalculateDamagePatches
    {
        //[HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.OnTrigger))]
        //[HarmonyPostfix]
        //public static void RuleCalculateDamage_OnTrigger_Postfix(RuleCalculateDamage __instance)
        //{
        //    foreach (var bundle in __instance.CalculatedDamage)
        //    {
        //        Main.GetLogger<RuleCalculateDamagePatches>().LogWarning("Final={Final}, Reduction={Reduction}, RollAndBonus={RollAndBonus}, Roll={Roll}, DRModifier={DRModifier}, ValueWithoutRed={ValueWithoutRed}", bundle.FinalValue, bundle.Reduction, bundle.RollAndBonusValue, bundle.RollResult, bundle.TacticalCombatDRModifier, bundle.ValueWithoutReduction);
        //    }
        //}

        [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.Roll))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleCalculateDamage_Roll_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var lookFor = AccessTools.Method(typeof(Dice), nameof(Dice.D), [typeof(DiceFormula)]);
            var replaceWith = AccessTools.Method(typeof(RuleCalculateDamagePatches), nameof(RuleCalculateDamagePatches.OnCalculateDamageRoll));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleCalculateDamagePatches>().LogError("Transpiler has not been applied (ReplaceNonTacticalCombat). Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstruction().Insert(newInstructions);
            Main.GetLogger<RuleCalculateDamagePatches>().LogDebug("Transpiler has been applied (ReplaceNonTacticalCombat). Target={Target}", target);
            return matcher.Instructions();
        }

        private static int OnCalculateDamageRoll(DiceFormula diceFormula, RuleCalculateDamage ruleCalculateDamage)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Dice.D(diceFormula);
            }

            int? roll = Main.Rolls.OnBeforeRuleCalculateDamageRoll(ruleCalculateDamage, diceFormula);
            if (roll.HasValue)
            {
                return roll.Value;
            }

            return Dice.D(diceFormula);
        }
    }
}
