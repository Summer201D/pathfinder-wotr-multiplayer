using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Controllers.Combat;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class UnitCombatPrepareControllerPatches
    {
        /// <summary>
        /// Removes random initiative
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(UnitCombatPrepareController), nameof(UnitCombatPrepareController.Tick))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> UnitCombatPrepareController_Tick_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var lookFor = AccessTools.PropertySetter(typeof(UnitCombatState), nameof(UnitCombatState.InitiativeRandom));
            CodeMatcher match;
            int replacementCounter = 0;
            while ((match = matcher.SearchForward(x => x.Calls(lookFor))).IsValid)
            {
                match = match.Advance(-8);
                match.RemoveInstructions(9);
                var newInstructions = new List<CodeInstruction>
                {
                    new (OpCodes.Ldc_I4_1)
                };
                match.Insert(newInstructions);
                replacementCounter++;
            }

            const int ExpectedReplacementCounter = 2;
            if (replacementCounter != ExpectedReplacementCounter)
            {
                Main.GetLogger<TurnControllerPatches>().LogError("Instructions have not been replaced expected number of times. Target={Target}, Expected={expected}, Current={current}", target, ExpectedReplacementCounter, replacementCounter);
                return instructions;
            }

            Main.GetLogger<TurnControllerPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }
    }
}
