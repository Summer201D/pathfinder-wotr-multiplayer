using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases.Pregen;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class CharGenPregenPatches
    {
        [HarmonyPatch(typeof(CharGenPregenPhaseDetailedPCView), nameof(CharGenPregenPhaseDetailedPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void CharGenPregenPhaseDetailedPCView_BindViewImplementation_Postfix(CharGenPregenPhaseDetailedPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            foreach (var item in __instance.m_PregenSelectorPCView.VirtualList.Elements)
            {
                item.IsActive = false;
            }

            __instance.m_CreateCustomCharacterButton.m_OnLeftClick.Invoke();
        }
    }
}
