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
    public class RuleHealDamagePatches
    {
        [HarmonyPatch(typeof(RuleHealDamage), nameof(RuleHealDamage.Roll))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleHealDamage_Roll_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var lookFor = AccessTools.Method(typeof(Dice), nameof(Dice.D), [typeof(DiceFormula)]);
            var replaceWith = AccessTools.Method(typeof(RuleHealDamagePatches), nameof(RuleHealDamagePatches.HealDamageRoll));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleHealDamagePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstruction().Insert(newInstructions);
            Main.GetLogger<RuleHealDamagePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);

            return matcher.Instructions();
        }

        private static int HealDamageRoll(DiceFormula diceFormula, RuleHealDamage ruleHealDamage)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Dice.D(diceFormula);
            }

            var shouldRunOriginalLogic = Main.Rolls.OnBeforeRollRuleHealDamage(ruleHealDamage, diceFormula);
            if (!shouldRunOriginalLogic)
            {
                return ruleHealDamage.RollResult;
            }

            return Dice.D(diceFormula);
        }
    }
}
