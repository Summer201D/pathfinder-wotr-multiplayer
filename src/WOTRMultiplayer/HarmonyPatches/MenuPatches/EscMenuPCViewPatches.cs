using System;
using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.EscMenu;
using Microsoft.Extensions.Logging;
using Owlcat.Runtime.UI.Controls.Button;

namespace WOTRMultiplayer.HarmonyPatches.MenuPatches
{
    [HarmonyPatch]
    public class EscMenuPCViewPatches
    {
        [HarmonyPatch(typeof(EscMenuPCView), nameof(EscMenuPCView.BindViewImplementation))]
        [HarmonyPrefix]
        public static void EscMenuPCView_BindViewImplementation_Prefix(EscMenuPCView __instance)
        {
            try
            {
                if (Main.Lobby.Window == null && Main.Multiplayer.IsActive)
                {
                    Main.Lobby.EnsureStandaloneWindowInitialized();
                    SetPhotoModeButtonState(__instance, false);
                }
                else if (Main.Lobby.Window != null && !Main.Multiplayer.IsActive)
                {
                    Main.Multiplayer.TerminateMultiplayer();
                    SetPhotoModeButtonState(__instance, true);
                }
            }
            catch (Exception ex)
            {
                Main.GetLogger<EscMenuPCViewPatches>().LogError(ex, "Unable to apply patch");
                throw;
            }
        }

        private static void SetPhotoModeButtonState(EscMenuPCView view, bool isInteractable)
        {
            var photoMode = view.transform.Find("Window/ButtonBlock/PhotoModeButton")?.gameObject;
            if (photoMode != null)
            {
                photoMode.GetComponent<OwlcatButton>().Interactable = isInteractable;
            }
        }
    }
}
