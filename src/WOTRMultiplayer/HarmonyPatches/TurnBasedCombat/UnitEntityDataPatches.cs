using HarmonyLib;
using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.HarmonyPatches.TurnBasedCombat
{
    [HarmonyPatch]
    public class UnitEntityDataPatches
    {
        [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.JoinCombat))]
        [HarmonyPrefix]
        public static bool UnitEntityData_JoinCombat_Prefix(UnitEntityData __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            return Main.Multiplayer.CanUnitJoinCombat(__instance.UniqueId);
        }
    }
}
