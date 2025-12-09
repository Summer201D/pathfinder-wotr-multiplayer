using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.GlobalMap.Message;
using Kingmaker.UI.MVVM._VM.GlobalMap.Message;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapRandomEncounterPatches
    {
        [HarmonyPatch(typeof(GlobalMapRandomEncounterPCView), nameof(GlobalMapRandomEncounterPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void GlobalMapRandomEncounterPCView_BindViewImplementation_Postfix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapEncounterMessageShown();
        }

        [HarmonyPatch(typeof(GlobalMapRandomEncounterView), nameof(GlobalMapRandomEncounterView.AcceptAndStopCoroutine))]
        [HarmonyPrefix]
        public static void GlobalMapRandomEncounterView_AcceptAndStopCoroutine_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapEncounterAccepted();
        }

        [HarmonyPatch(typeof(GlobalMapRandomEncounterVM), nameof(GlobalMapRandomEncounterVM.Avoid))]
        [HarmonyPrefix]
        public static void GlobalMapRandomEncounterVM_Avoid_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapEncounterAvoided();
        }
    }
}
