using HarmonyLib;
using Kingmaker.Settings;
using Kingmaker.UI.MVVM._PCView.NewGame.Story;
using Kingmaker.UI.MVVM._VM.NewGame.Story;
using UniRx;
using WOTRMultiplayer.Entities.NewGame;

namespace WOTRMultiplayer.HarmonyPatches.NewGameSequence
{
    [HarmonyPatch]
    public class NewGamePhaseStoryVMPatches
    {
        [HarmonyPatch(typeof(NewGamePhaseStoryPCView), nameof(NewGamePhaseStoryPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void NewGamePhaseStoryPCView_BindViewImplementation_Postfix(NewGamePhaseStoryPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.AddDisposable(__instance.m_SelectorPCView.ViewModel.SelectedEntity.Subscribe(story =>
            {
                var campaign = Main.Mapper.Map<NetworkCampaign>(story.m_StoryCampaign);
                Main.Multiplayer.OnNewGameSequenceCampaignChanged(campaign);
            }));
            __instance.AddDisposable(__instance.ViewModel.LastAzlantiIsOn.Subscribe(isEnabled =>
            {
                Main.Multiplayer.OnNewGameSequenceLastAzlantiChanged(isEnabled);
            }));

            __instance.m_LastAzlantiButton.Interactable = Main.Multiplayer.CanMakeNewGameSequenceDecisions();
        }

        [HarmonyPatch(typeof(NewGamePhaseStoryVM), nameof(NewGamePhaseStoryVM.OnNext))]
        [HarmonyPrefix]
        public static void NewGamePhaseStoryVM_OnNext_Prefix(NewGamePhaseStoryVM __instance)
        {
            if (!Main.Multiplayer.IsActive || !__instance.IsButtonNextAvailable.Value || !__instance.m_SelectedEntity.Value.IsDungeon || SettingsRoot.Game.Main.DLC3Warning)
            {
                return;
            }

            SettingsRoot.Game.Main.DLC3Warning.SetValueAndConfirm(true);
            SettingsController.SaveAll();
        }
    }
}
