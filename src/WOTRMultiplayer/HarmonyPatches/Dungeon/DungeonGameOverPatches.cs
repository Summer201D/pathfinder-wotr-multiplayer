using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Dungeon;
using Kingmaker.UI.MVVM._VM.Dungeon;
using Owlcat.Runtime.UI.Controls.Other;
using UniRx;

namespace WOTRMultiplayer.HarmonyPatches.Dungeon
{
    [HarmonyPatch]
    public class DungeonGameOverPatches
    {
        [HarmonyPatch(typeof(DungeonGameOverPCView), nameof(DungeonGameOverPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void DungeonGameOverPCView_BindViewImplementation_Postfix(DungeonGameOverPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.AddDisposable(__instance.m_StartNewGameButton.OnLeftClickAsObservable().Subscribe(_ =>
            {
                Main.Multiplayer.OnDungeonGameOverStartNewGame();
            }));

            __instance.AddDisposable(__instance.m_LoadLatestSaveButton.OnLeftClickAsObservable().Subscribe(_ =>
            {
                Main.Multiplayer.OnDungeonGameOverLoadLatestSave();
            }));

            Main.Multiplayer.OnDungeonGameOverShown();
        }

        [HarmonyPatch(typeof(DungeonGameOverVM), nameof(DungeonGameOverVM.OnButtonMainMenu))]
        [HarmonyPrefix]
        public static bool DungeonGameOverVM_OnButtonMainMenu_Prefix(DungeonGameOverVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            Main.Multiplayer.OnDungeonGameOverGoToMainMenu();
            __instance.GoToMainMenu(Kingmaker.UI.MessageModalBase.ButtonType.Yes);
            return false;
        }
    }
}
