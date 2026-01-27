using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.MVVM._VM.IngameMenu;

namespace WOTRMultiplayer.HarmonyPatches.MenuPatches
{
    [HarmonyPatch]
    public class IngameMenuPatches
    {
        [HarmonyPatch(typeof(IngameMenuVM), nameof(IngameMenuVM.SwitchTBM))]
        [HarmonyPrefix]
        public static bool IngameMenuVM_SwitchTBM_Prefix()
        {
            return !Main.Multiplayer.IsActive;
        }

        [HarmonyPatch(typeof(IngameMenuVM), nameof(IngameMenuVM.OpenRestCamp))]
        [HarmonyPrefix]
        public static void IngameMenuVM_OpenRestCamp_Prefix()
        {
            if (!Main.Multiplayer.IsActive || !Game.Instance.Player.CapitalPartyMode)
            {
                return;
            }

            Main.Multiplayer.OnCapitalModeRest();
        }
    }
}
