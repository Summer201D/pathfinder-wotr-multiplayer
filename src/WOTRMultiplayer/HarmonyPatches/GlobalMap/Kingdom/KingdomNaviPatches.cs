using HarmonyLib;
using Kingmaker.UI.Kingdom;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap.Kingdom
{
    [HarmonyPatch]
    public class KingdomNaviPatches
    {
        [HarmonyPatch(typeof(KingdomNaviElement), nameof(KingdomNaviElement.OnClickHandler))]
        [HarmonyPrefix]
        public static bool KingdomNaviElement_OnClickHandler_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanControlGlobalMap();
            return canContinue;
        }

        [HarmonyPatch(typeof(KingdomNaviController), nameof(KingdomNaviController.OnHotKeyShow))]
        [HarmonyPrefix]
        public static bool KingdomNaviController_OnHotKeyShow_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanControlGlobalMap();
            return canContinue;
        }
    }
}
