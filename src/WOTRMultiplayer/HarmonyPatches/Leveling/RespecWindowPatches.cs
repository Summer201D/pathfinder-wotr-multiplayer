using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.CharGen;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class RespecWindowPatches
    {
        [HarmonyPatch(typeof(RespecWindowPCView), nameof(RespecWindowPCView.SetupMythicUpButtonView))]
        [HarmonyPostfix]
        public static void RespecWindowPCView_SetupMythicUpButtonView_Postfix(RespecWindowPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.m_MaxMythicUpButton.Interactable = false;
        }

        [HarmonyPatch(typeof(RespecWindowPCView), nameof(RespecWindowPCView.SetupLevelupButtonView))]
        [HarmonyPostfix]
        public static void RespecWindowPCView_SetupLevelupButtonView_Postfix(RespecWindowPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.m_MaxLevelupButton.Interactable = false;
        }
    }
}
