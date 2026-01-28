using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleRollChancePatches
    {
        [HarmonyPatch(typeof(RuleRollChance), nameof(RuleRollChance.OnTrigger))]
        [HarmonyPostfix]
        public static void RuleRollChance_OnTrigger_Postfix(RuleRollChance __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Rolls.OnAfterRuleRollChanceTrigger(__instance);
        }

        [HarmonyPatch(typeof(RuleRollChance), nameof(RuleRollChance.OnTrigger))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleRollChance_OnTrigger_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RuleRollChancePatches), nameof(RuleRollChancePatches.OnTrigger));
            var lookFor = AccessTools.Method(typeof(RuleRollDice), nameof(RuleRollDice.OnTrigger));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleRollChancePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };

            match = match.Advance(1).Insert(newInstructions);
            Main.GetLogger<RuleRollChancePatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static void OnTrigger(RuleRollChance ruleRollChance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // techically it's after trigger, but before any rolled value is taken into account
            Main.Rolls.OnBeforeRuleRollChanceTrigger(ruleRollChance);
        }
    }
}
