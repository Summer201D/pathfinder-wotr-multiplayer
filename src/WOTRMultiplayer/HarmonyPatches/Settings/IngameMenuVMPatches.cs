using HarmonyLib;
using Kingmaker.UI.MVVM._VM.IngameMenu;

namespace WOTRMultiplayer.HarmonyPatches.Settings
{
    [HarmonyPatch]
    public class IngameMenuVMPatches
    {
        [HarmonyPatch(typeof(IngameMenuVM), nameof(IngameMenuVM.SwitchTBM))]
        [HarmonyPrefix]
        public static bool IngameMenuVM_SwitchTBM_Prefix()
        {
            return !Main.Multiplayer.IsActive;
        }
    }
}
