using HarmonyLib;
using Kingmaker.Controllers.Combat;
using Microsoft.Extensions.Logging;
using TurnBased.Controllers;

namespace WOTRMultiplayer.HarmonyPatches.TurnBasedCombat
{
    // order after host allowed to start combat
    // UnitCombatPrepareController.Tick -> CombatController_Reset -> CombatController_Tick
    [HarmonyPatch]
    public class TurnBasedCombatPatches
    {
        [HarmonyPatch(typeof(UnitCombatPrepareController), nameof(UnitCombatPrepareController.Tick))]
        [HarmonyPrefix]
        public static bool UnitCombatPrepareController_Tick_Prefix(UnitCombatPrepareController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // block on client until confirmation from host
            // rolls initiatives which are required
            var canContinue = Main.Multiplayer.CanTickUnitCombatPrepareController();
            return canContinue;
        }

        [HarmonyPatch(typeof(CombatController), nameof(CombatController.Tick))]
        [HarmonyPrefix]
        public static bool CombatController_Tick_Prefix(CombatController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // host - confirm initialization with clients
            // client - always true
            var canContinue = Main.Multiplayer.CanTickCombatController();
            return canContinue;
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.Start))]
        [HarmonyPrefix]
        public static bool TurnController_Start_Prefix(TurnController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            Main.GetLogger<TurnBasedCombatPatches>().LogInformation("TurnController_Start_Prefix. CharacterName={characterName}, UnitId={unitId}", __instance.Rider.CharacterName, __instance.Rider.UniqueId);
            // block on host/client until everyone is not trying to start same turn
            var canContinue = Main.Multiplayer.OnBeforeStartTurn(__instance.Rider.UniqueId);
            return canContinue;
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.End))]
        [HarmonyPrefix]
        public static bool TurnController_End_Prefix(TurnController __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            Main.GetLogger<TurnBasedCombatPatches>().LogInformation("TurnController_End_Prefix. CharacterName={characterName}, UnitId={unitId}", __instance.Rider.CharacterName, __instance.Rider.UniqueId);

            var canContinue = Main.Multiplayer.OnBeforeEndTurn(__instance.Rider.UniqueId);
            return canContinue;
        }
    }
}
