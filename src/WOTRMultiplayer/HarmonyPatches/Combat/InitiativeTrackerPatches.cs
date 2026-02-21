using HarmonyLib;
using Kingmaker.UI._ConsoleUI.TurnBasedMode;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class InitiativeTrackerPatches
    {
        [HarmonyPatch(typeof(InitiativeTrackerVM), nameof(InitiativeTrackerVM.InterruptMovement))]
        [HarmonyPrefix]
        public static bool InitiativeTrackerVM_InterruptMovement_Prefix()
        {
            return !Main.Multiplayer.IsActive;
        }
    }
}
