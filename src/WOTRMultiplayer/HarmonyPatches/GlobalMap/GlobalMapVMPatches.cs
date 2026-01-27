using HarmonyLib;
using Kingmaker.UI.MVVM._VM.GlobalMap;

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
    }
}
