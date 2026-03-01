using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Microsoft.Extensions.Logging;
using static Kingmaker.RuleSystem.RulebookEvent;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleCheckConcentrationPatches
    {
        [HarmonyPatch(typeof(RuleCheckConcentration), nameof(RuleCheckConcentration.OnTrigger))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleCheckConcentration_OnTrigger_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RuleCheckConcentrationPatches), nameof(RuleCheckConcentrationPatches.CheckConcentrationRollD20));
            var lookFor = AccessTools.PropertyGetter(typeof(Dice), nameof(Dice.D20));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleCheckConcentrationPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith)
            };

            match = match.RemoveInstruction().Insert(newInstructions);
            Main.GetLogger<RuleCheckConcentrationPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);

            return matcher.Instructions();
        }

        private static RuleRollD20 CheckConcentrationRollD20(RuleCheckConcentration ruleCheckConcentration)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Dice.D20;
            }

            var shouldRunOriginalLogic = Main.Rolls.OnBeforeRuleCheckConcentrationRoll(ruleCheckConcentration);
            if (!shouldRunOriginalLogic)
            {
                return ruleCheckConcentration.ResultRollRaw;
            }

            return Dice.D20;
        }
    }
}
