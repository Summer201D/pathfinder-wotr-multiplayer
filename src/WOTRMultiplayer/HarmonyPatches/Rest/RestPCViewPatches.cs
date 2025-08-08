using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Rest;

namespace WOTRMultiplayer.HarmonyPatches.Rest
{
    [HarmonyPatch]
    public class RestPCViewPatches
    {
        /// <summary>
        /// 'Use Spells' toggle is a field sadly, so this patch is required
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(RestPCView), nameof(RestPCView.SetHealingState))]
        [HarmonyPrefix]
        public static bool RestPCView_SetHealingState_Prefix(RestPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }


            var shouldContinue = Main.Multiplayer.OnCampingUseHealingSpellsChanged(__instance.m_HealingToggle.isOn);
            return shouldContinue;
        }

        /// <summary>
        /// 'Use Spells' toggle is a field sadly, so this patch is required
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(RestPCView), nameof(RestPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void RestPCView_BindViewImplementation_Postfix(RestPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

        }
    }
}
