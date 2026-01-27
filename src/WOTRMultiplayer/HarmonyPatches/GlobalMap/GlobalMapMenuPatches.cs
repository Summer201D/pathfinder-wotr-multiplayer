using HarmonyLib;
using Kingmaker.Globalmap.View;
using Kingmaker.UI.MVVM._VM.GlobalMap.Menu;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapMenuPatches
    {
        [HarmonyPatch(typeof(GlobalMapMenuVM), nameof(GlobalMapMenuVM.OpenRestCamp))]
        [HarmonyPrefix]
        public static bool GlobalMapMenuVM_OpenRestCamp_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canUse = Main.UIAccessor.GlobalMapPCView?.m_GlobalMapMenuPCView?.m_RestButton.Interactable ?? false;
            if (canUse)
            {
                Main.Multiplayer.OnGlobalMapRestOpened();
            }

            return canUse;
        }

        [HarmonyPatch(typeof(GlobalMapView), nameof(GlobalMapView.StartChangedPartyOnGlobalMap))]
        [HarmonyPrefix]
        public static void GlobalMapView_StartChangedPartyOnGlobalMap_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapGroupChangerOpened();
        }

        [HarmonyPatch(typeof(GlobalMapMenuVM), nameof(GlobalMapMenuVM.OpenGroupChanger))]
        [HarmonyPrefix]
        public static bool GlobalMapMenuVM_OpenGroupChanger_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canUse = Main.UIAccessor.GlobalMapPCView?.m_GlobalMapMenuPCView?.m_GroupManagerButton.Interactable ?? false;
            return canUse;
        }

        [HarmonyPatch(typeof(GlobalMapMenuVM), nameof(GlobalMapMenuVM.SkipTime))]
        [HarmonyPrefix]
        public static bool GlobalMapMenuVM_SkipTime_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canUse = Main.UIAccessor.GlobalMapPCView?.m_GlobalMapMenuPCView?.m_SkipTime.Interactable ?? false;
            return canUse;
        }
    }
}
