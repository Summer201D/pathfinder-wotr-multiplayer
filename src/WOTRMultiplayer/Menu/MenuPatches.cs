using HarmonyLib;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._PCView.ContextMenu;
using Kingmaker.UI.MVVM._PCView.MainMenu;
using UnityEngine;

namespace WOTRMultiplayer.Menu
{
    [HarmonyPatch]
    public class MenuPatches
    {
        [HarmonyPatch(typeof(MainMenuPCView), "BindViewImplementation")]
        [HarmonyPrefix]
        public static void MainMenuPCView_BindViewImplementation_Prefix(MainMenuPCView __instance)
        {
            Logging.Logger.Info("Applying");
            var menuButtons = __instance.m_MainMenuSideBarPCView.transform.GetChild(0);
            var multiplayerMenu = Object.Instantiate(menuButtons.GetChild(0).gameObject, menuButtons.transform);
            multiplayerMenu.transform.SetSiblingIndex(3);
            var button = multiplayerMenu.GetComponent<ContextMenuEntityPCView>();
            button.m_Label.text = UIUtility.GetSaberBookFormat("Multiplayer");
        }
    }
}
