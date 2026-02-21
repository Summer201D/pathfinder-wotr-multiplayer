using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Microsoft.Extensions.Logging;
using TurnBased.Controllers;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class TurnControllerPatches
    {
        [HarmonyPatch(typeof(TurnController), nameof(TurnController.TryScrollToUnit))]
        [HarmonyPrefix]
        public static bool TurnController_TryScrollToUnit_Prefix(TurnController __instance, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // fails with NRE anyway
            if (__instance.Rider == null)
            {
                __result = false;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.CanEndTurn))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TurnController_CanEndTurn_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);

            CommonTranspilerReplacements.ReplaceIsDirectlyControllableWithLocalPlayerCheck(matcher, target, true);

            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.CanDelay))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TurnController_CanDelay_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);

            CommonTranspilerReplacements.ReplaceIsDirectlyControllableWithLocalPlayerCheck(matcher, target, true);

            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.Tick))]
        [HarmonyPrefix]
        public static bool TurnController_Tick_Prefix(TurnController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            return __instance.Rider != null;
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.Start))]
        [HarmonyPrefix]
        public static bool TurnController_Start_Prefix(TurnController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            return __instance.Rider != null;
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.End))]
        [HarmonyPrefix]
        public static bool TurnController_End_Prefix(TurnController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.OnBeforeTurnEnd(__instance.Rider?.UniqueId);
            return canContinue;
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.HandlePortraitHover))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TurnController_HandlePortraitHover_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!SetCallUpdateActionPredictionsIfControlled(matcher))
            {
                Main.GetLogger<TurnControllerPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            Main.GetLogger<TurnControllerPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.HandleOvertipHover))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TurnController_HandleOvertipHover_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!SetCallUpdateActionPredictionsIfControlled(matcher))
            {
                Main.GetLogger<HarmonyTranspiler>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            Main.GetLogger<TurnControllerPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static bool SetCallUpdateActionPredictionsIfControlled(CodeMatcher matcher)
        {
            var replaceWith = AccessTools.Method(typeof(TurnControllerPatches), nameof(TurnControllerPatches.UpdateActionPredictionsIfControlled));
            var lookFor = AccessTools.Method(typeof(TurnController), nameof(TurnController.UpdateActionPredictions));
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (!match.IsValid)
            {
                return false;
            }

            match = match
                .Advance(-1)
                .RemoveInstructions(2);

            var newInstructions = new List<CodeInstruction>
            {
                new(OpCodes.Call, replaceWith)
            };
            match.Insert(newInstructions);

            return true;
        }

        private static void UpdateActionPredictionsIfControlled()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var currentUnit = Game.Instance.TurnBasedCombatController.CurrentTurn?.Rider;
            if (currentUnit != null && Main.Multiplayer.IsControlledByLocalPlayer(currentUnit.UniqueId))
            {
                Game.Instance.TurnBasedCombatController.CurrentTurn.UpdateActionPredictions();
            }
        }
    }
}
