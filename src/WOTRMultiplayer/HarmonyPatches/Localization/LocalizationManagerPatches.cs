using HarmonyLib;
using Kingmaker.Localization;

namespace WOTRMultiplayer.HarmonyPatches.Localization
{
    [HarmonyPatch]
    public class LocalizationManagerPatches
    {
        [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.OnLocaleChanged))]
        [HarmonyPrefix]
        public static void LocalizationManager_OnLocaleChanged_Prefix()
        {
            var locale = LocalizationManager.CurrentLocale.ToString();
            Main.UpdateLocale(locale);
        }
    }
}
