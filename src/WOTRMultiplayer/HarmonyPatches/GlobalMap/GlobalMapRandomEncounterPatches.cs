using HarmonyLib;
using Kingmaker.Globalmap.State;
using Kingmaker.UI.GlobalMap;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapRandomEncounterPatches
    {
        [HarmonyPatch(typeof(GlobalMapRandomEncounterController), nameof(GlobalMapRandomEncounterController.OnRandomEncounterStarted))]
        [HarmonyPrefix]
        public static void GlobalMapRandomEncounterController_OnRandomEncounterStarted_Prefix(GlobalMapRandomEncounterController __instance)
        {
            Main.GetLogger<GlobalMapRandomEncounterPatches>().LogWarning("On Random Encounter");
        }

        [HarmonyPatch(typeof(GlobalMapMessageBox), nameof(GlobalMapMessageBox.OnLocationSelect))]
        [HarmonyPrefix]
        public static void GlobalMapMessageBox_OnLocationSelect_Prefix(GlobalMapMessageBox __instance)
        {
            Main.GetLogger<GlobalMapRandomEncounterPatches>().LogWarning("GlobalMapMessageBox_OnLocationSelect_Prefix");
        }

        [HarmonyPatch(typeof(GlobalMapLocationInfo), nameof(GlobalMapLocationInfo.HandleLocationHover))]
        [HarmonyPrefix]
        public static void GlobalMapLocationInfo_HandleLocationHover_Prefix(GlobalMapLocationInfo __instance, GlobalMapPointState locationData, bool hover)
        {
            if (!Main.Multiplayer.IsActive || !hover)
            {
                return;
            }

            Main.GetLogger<GlobalMapRandomEncounterPatches>().LogWarning("GlobalMapLocationInfo_HandleLocationHover_Prefix");
        }
    }
}
