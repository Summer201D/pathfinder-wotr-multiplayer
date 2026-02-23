using HarmonyLib;
using Kingmaker.UI.MVVM._VM.GlobalMap;
using Kingmaker.UI.MVVM._VM.Teleport;
using WOTRMultiplayer.Entities.GlobalMap;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapVMPatches
    {
        /// <summary>
        ///  Base game is missing some dispose calls. This causes some Esc subscriptions to become invalid
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(GlobalMapVM), nameof(GlobalMapVM.DisposeImplementation))]
        [HarmonyPostfix]
        public static void GlobalMapVM_DisposeImplementation_Postfix(GlobalMapVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.DisposeLeaderLevelup();
        }

        [HarmonyPatch(typeof(GlobalMapVM), nameof(GlobalMapVM.OnInitSelectDestination))]
        [HarmonyPrefix]
        public static bool GlobalMapVM_OnInitSelectDestination_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanControlGlobalMap();
            return canContinue;
        }

        [HarmonyPatch(typeof(TeleportEntityVM), nameof(TeleportEntityVM.OnTeleport))]
        [HarmonyPrefix]
        public static void TeleportEntityVM_OnTeleport_Prefix(TeleportEntityVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var teleportLocation = new NetworkGlobalMapLocation
            {
                Id = __instance.m_Location.Blueprint.AssetGuid.ToString(),
                Name = __instance.m_Location.Blueprint.name
            };

            Main.Multiplayer.OnGlobalMapTeleport(teleportLocation);
        }
    }
}
