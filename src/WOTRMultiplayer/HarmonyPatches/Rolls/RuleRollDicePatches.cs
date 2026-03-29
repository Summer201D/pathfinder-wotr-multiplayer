using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleRollDicePatches
    {
        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.Reroll), [])]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleRollDice_Reroll_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RuleRollDicePatches), nameof(RuleRollDicePatches.Roll));
            var lookFor = AccessTools.Method(typeof(RulebookEvent.Dice), nameof(RulebookEvent.Dice.D), [typeof(DiceFormula)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleRollDicePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };

            match = match.RemoveInstructions(1).Insert(newInstructions);
            Main.GetLogger<RuleRollDicePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.Roll))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleRollDice_Roll_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RuleRollDicePatches), nameof(RuleRollDicePatches.Roll));
            var lookFor = AccessTools.Method(typeof(RulebookEvent.Dice), nameof(RulebookEvent.Dice.D), [typeof(DiceFormula)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleRollDicePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };

            match = match.RemoveInstructions(1).Insert(newInstructions);
            Main.GetLogger<RuleRollDicePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.PreRollDice))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleRollDice_PreRollDice_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RuleRollDicePatches), nameof(RuleRollDicePatches.Roll));
            var lookFor = AccessTools.Method(typeof(RulebookEvent.Dice), nameof(RulebookEvent.Dice.D), [typeof(DiceFormula)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleRollDicePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };

            match = match.RemoveInstructions(1).Insert(newInstructions);
            Main.GetLogger<RuleRollDicePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static int Roll(DiceFormula diceFormula, RuleRollDice ruleRollDice)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return RulebookEvent.Dice.D(diceFormula);
            }

            var value = Main.Rolls.OnRollDice(ruleRollDice);
            if (value == null)
            {
                return RulebookEvent.Dice.D(diceFormula);
            }

            return value.Value;
        }
    }
}
