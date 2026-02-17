using System;
using HarmonyLib;
using Kingmaker.Controllers.Units;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class SaddledUnitControllerPatches
    {
        [HarmonyPatch(typeof(SaddledUnitController), nameof(SaddledUnitController.TickDelegateMountToRider))]
        [HarmonyPrefix]
        public static bool SaddledUnitController_TickDelegateMountToRider_Prefix(UnitPartSaddled mountPart)
        {
            if (!Main.Multiplayer.IsActive
                || !Main.Multiplayer.IsControlledByPlayers(mountPart.Owner.UniqueId)
                || Main.Multiplayer.IsControlledByLocalPlayer(mountPart.Owner.UniqueId)
                || !Main.Multiplayer.IsInCombat)
            {
                return true;
            }

            try
            {
                var command = mountPart.Owner.Commands.Raw.FirstItem(c => c != null && !c.IsActed && c.AiAction == null && c.RiderCommand == null && c is not UnitAttackOfOpportunity);
                if (command == null)
                {
                    return true;
                }

                var canContinue = CanDelegateCommand(command);
                return canContinue;
            }
            catch (Exception ex)
            {
                Main.GetLogger<SaddledUnitControllerPatches>().LogError(ex, "Error while ticking mount->rider delegate");
                throw;
            }
        }

        [HarmonyPatch(typeof(SaddledUnitController), nameof(SaddledUnitController.TickDelegateRiderToMount))]
        [HarmonyPrefix]
        public static bool SaddledUnitController_TickDelegateRiderToMount_Prefix(UnitPartRider riderPart)
        {
            if (!Main.Multiplayer.IsActive
                || !Main.Multiplayer.IsControlledByPlayers(riderPart.Owner.UniqueId)
                || Main.Multiplayer.IsControlledByLocalPlayer(riderPart.Owner.UniqueId)
                || !Main.Multiplayer.IsInCombat)
            {
                return true;
            }

            try
            {
                var command = riderPart.Owner.Commands.Raw.FirstItem(c => c != null && !c.IsActed && c is not UnitAttackOfOpportunity);
                if (command == null)
                {
                    return true;
                }

                var canContinue = CanDelegateCommand(command);
                return canContinue;
            }
            catch (Exception ex)
            {
                Main.GetLogger<SaddledUnitControllerPatches>().LogError(ex, "Error while ticking rider->mount delegate");
                throw;
            }
        }

        private static bool CanDelegateCommand(UnitCommand unitCommand)
        {
            var canDelegate = unitCommand switch
            {
                // TODO: CreatedByPlayer is set to true by ClickGroundHandler, but it's kinda obsolete and should be replaced with direct syncing of unitmoveto commands
                UnitMoveTo moveTo => moveTo.CreatedByPlayer,
                _ => !unitCommand.CreatedByPlayer,
            };
            return canDelegate;
        }
    }
}
