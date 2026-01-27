using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Crusade.ArmyInfo;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class ArmyInfoHUDLeaderViewPatches
    {
        [HarmonyPatch(typeof(ArmyInfoHUDLeaderView), nameof(ArmyInfoHUDLeaderView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void ArmyInfoHUDLeaderView_BindViewImplementation_Postfix(ArmyInfoHUDLeaderView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.m_LevelupButton.Interactable = __instance.m_LevelupButton.Interactable && Main.Multiplayer.CanControlGlobalMap();
        }
    }
}
