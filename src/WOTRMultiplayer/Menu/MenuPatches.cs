using System;
using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._PCView.ContextMenu;
using Kingmaker.UI.MVVM._PCView.MainMenu;
using Kingmaker.UI.MVVM._VM.ContextMenu;
using Kingmaker.UI.ServiceWindow.Credits;
using WOTRMultiplayer.Strings;

namespace WOTRMultiplayer.Menu
{
    [HarmonyPatch]
    public class MenuPatches
    {
        [HarmonyPatch(typeof(MainMenuSideBarPCView), "BindViewImplementation")]
        [HarmonyPrefix]
        public static void MainMenuSideBarPCView_BindViewImplementation_Prefix(MainMenuSideBarPCView __instance)
        {
            Logging.Logger.Info("Applying");
            try
            {
                var menuButtons = __instance.transform.GetChild(0);
                var menuItemToCopy = menuButtons.GetChild(3).gameObject;
                var multiplayerMenu = UnityEngine.Object.Instantiate(menuItemToCopy, menuButtons.transform);
                multiplayerMenu.transform.SetSiblingIndex(3);
                var multiplayerMenuView = multiplayerMenu.GetComponent<ContextMenuEntityPCView>();
                var window = CreateMultiplayerWindow();
                var text = UIUtility.GetSaberBookFormat(StringConsts.MainMenu.MultiplayerMenu);
                var viewModel = new ContextMenuEntityVM(new ContextMenuCollectionEntity(UIUtility.GetSaberBookFormat(text), () => window.Show(true)));
                multiplayerMenuView.Bind(viewModel);
            }
            catch (Exception ex)
            {
                Logging.Logger.Error("Unable to apply patch", ex);
                throw;
            }
        }

        private static MultiplayerWindow CreateMultiplayerWindow()
        {
            Logging.Logger.Info($"Creating new instance of {nameof(MultiplayerWindow)}");

            var copy = UnityEngine.Object.Instantiate(Game.Instance.UI.CreditsUI.gameObject, Game.Instance.UI.MainMenu.transform);
            var originalWindow = copy.GetComponent<CreditsUIWindow>();
            var mainMenuMultiplayerWindow = copy.AddComponent<MultiplayerWindow>();
            UnityEngine.Object.DestroyImmediate(originalWindow);
            mainMenuMultiplayerWindow.Initialize();
            return mainMenuMultiplayerWindow;
        }
    }
}
