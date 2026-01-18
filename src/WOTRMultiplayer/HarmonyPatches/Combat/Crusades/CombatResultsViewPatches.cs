using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Crusade.CombatResult;
using Kingmaker.UI.MVVM._VM.Crusade.CombatResult;

namespace WOTRMultiplayer.HarmonyPatches.Combat.Crusades
{
    [HarmonyPatch]
    public class CombatResultsViewPatches
    {
        [HarmonyPatch(typeof(CombatResultPCView), nameof(CombatResultPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void CombatResultPCView_BindViewImplementation_Postfix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapCombatResultsShown();
        }

        [HarmonyPatch(typeof(CombatResultVM), nameof(CombatResultVM.Close))]
        [HarmonyPrefix]
        public static void CombatResultVM_Close_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapCombatResultsClosed();
        }
    }
}
