using HarmonyLib;
using Kingmaker.UI;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class PlayerUISettingsPatches
    {
        [HarmonyPatch(typeof(PlayerUISettings), nameof(PlayerUISettings.DoSpeedUp))]
        [HarmonyPrefix]
        public static bool PlayerUISettings_DoSpeedUp_Prefix()
        {
            return !Main.Multiplayer.IsActive;
        }
    }
}
