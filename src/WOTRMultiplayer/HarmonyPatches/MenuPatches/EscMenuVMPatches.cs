using HarmonyLib;
using Kingmaker.UI;
using Kingmaker.UI.MVVM._VM.EscMenu;

namespace WOTRMultiplayer.HarmonyPatches.MenuPatches
{
    [HarmonyPatch]
    public class EscMenuVMPatches
    {
        [HarmonyPatch(typeof(EscMenuVM), nameof(EscMenuVM.OnQuitToMainMenuAction))]
        [HarmonyPrefix]
        public static void EscMenuVM_OnQuitToMainMenuAction_Prefix(MessageModalBase.ButtonType buttonType)
        {
            if (buttonType == MessageModalBase.ButtonType.Yes)
            {
                Main.Multiplayer.TerminateMultiplayer();
            }
        }
    }
}
