using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Controllers;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Microsoft.Extensions.Logging;
using TurnBased.Controllers;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class CombatControllerPatches
    {
        [HarmonyPatch(typeof(CombatController), nameof(CombatController.HandleCombatStart))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CombatController_HandleCombatStart_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var replaceWith = AccessTools.Method(typeof(CombatControllerPatches), nameof(CombatControllerPatches.HasMoreThanOneSelectedUnit));
            var lookFor = AccessTools.PropertyGetter(typeof(SelectionCharacterController), nameof(SelectionCharacterController.SelectedUnits));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<CombatControllerPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match = match.Advance(-2).RemoveInstructions(6).Insert(newInstructions);

            Main.GetLogger<CombatControllerPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(UnitCombatPrepareController), nameof(UnitCombatPrepareController.Tick))]
        [HarmonyPrefix]
        public static bool UnitCombatPrepareController_Tick_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanTickUnitCombatPrepareController();
            return canContinue;
        }

        [HarmonyPatch(typeof(CombatController), nameof(CombatController.HandleDelayTurn))]
        [HarmonyPrefix]
        public static void CombatController_HandleDelayTurn_Prefix(UnitEntityData unit, UnitEntityData targetUnit)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnHandleDelayCombatTurn(unit.UniqueId, targetUnit.UniqueId);
        }

        [HarmonyPatch(typeof(CombatController), nameof(CombatController.Tick))]
        [HarmonyPrefix]
        public static bool CombatController_Tick_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanTickCombatController();
            return canContinue;
        }

        [HarmonyPatch(typeof(CombatController), nameof(CombatController.StartTurn))]
        [HarmonyPrefix]
        public static bool CombatController_StartTurn_Prefix(CombatController __instance, UnitEntityData unit)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            try
            {
                var unitInfo = __instance.FindUnitInfo(unit);
                var canContinue = Main.Multiplayer.OnBeforeTurnStart(unit.UniqueId, unitInfo?.ActingInSurpriseRound ?? false);
                if (!canContinue)
                {
                    // creating fake turn to restrict rechoosing unit / starting new turn before all the confirmations
                    __instance.CurrentTurn = new TurnController((JsonConstructorMark)default);
                    __instance.TurnStartTime = Game.Instance.TimeController.GameTime;
                }

                return canContinue;
            }
            catch (Exception ex)
            {
                Main.GetLogger<CombatControllerPatches>().LogError(ex, "Error while starting combat turn");
                throw;
            }
        }

        /// <summary>
        /// The game relies on a random number to determine turn order in cases where Initiative/Stats are the same. Unfortunately, this leads to a possible (50%) desync between MP clients
        /// This transpiler modifies the comparer to stop relying on CombatState.InitiativeRandom and instead compare UnitEntityData.UniqueId which produces same results on different PCs
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(CombatController.UnitsOrderComaprer), nameof(CombatController.UnitsOrderComaprer.Compare))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> UnitsOrderComaprer_Compare_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var lookFor = AccessTools.PropertyGetter(typeof(UnitCombatState), nameof(UnitCombatState.InitiativeRandom));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<CombatControllerPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return matcher.Instructions();
            }

            var actualValidPosition = matcher.Advance(-1);
            actualValidPosition.RemoveInstructions(matcher.Length - actualValidPosition.Pos - 1); // keep last `ret`
            var newInstructions = new List<CodeInstruction>()
            {
                // OpCodes.Ldloc_0 is already loaded (UnitEntityData xi)
                new(OpCodes.Ldloc_1), // (UnitEntityData yi)
                new(OpCodes.Call, AccessTools.Method(typeof(CombatControllerPatches), nameof(CombatControllerPatches.CompareUnitsByUniqueId)))
            };
            actualValidPosition.Insert(newInstructions);
            Main.GetLogger<CombatControllerPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);

            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(CombatController), nameof(CombatController.UpdateNavigationGridTags))]
        [HarmonyPrefix]
        public static bool CombatController_UpdateNavigationGridTags_Prefix(CombatController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // it fails with NRE anyway, just less error logs
            return AstarPath.active?.data?.gridGraph != null && __instance.CurrentTurn?.Rider != null;
        }

        private static int CompareUnitsByUniqueId(UnitEntityData xi, UnitEntityData yi)
        {
            try
            {
                var result = new[] { xi, yi }.OrderBy(x => x.UniqueId, StringComparer.OrdinalIgnoreCase).First() == xi ? -1 : 1;
                Main.GetLogger<CombatController.UnitsOrderComaprer>().LogInformation("Units have same initiave order, comparing by uniqueId. Result={result}, Unit1={unit1}, Unit2={unit2}", result, xi.UniqueId, yi.UniqueId);
                return result;
            }
            catch (Exception ex)
            {
                Main.GetLogger<CombatController.UnitsOrderComaprer>().LogError(ex, "Error while comparing by unique id. Unit1={unit1}, Unit2={unit2}", xi?.UniqueId, yi?.UniqueId);
                throw;
            }
        }

        private static bool HasMoreThanOneSelectedUnit()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Game.Instance.SelectionCharacter.SelectedUnits.Count > 1;
            }

            // there are multiple selected units while someone else starts surprise combat
            return false;
        }
    }
}
