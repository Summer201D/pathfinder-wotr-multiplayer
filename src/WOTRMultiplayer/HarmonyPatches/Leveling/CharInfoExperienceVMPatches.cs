using HarmonyLib;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.LevelClassScores.Experience;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class CharInfoExperienceVMPatches
    {
        [HarmonyPatch(typeof(CharInfoExperienceVM), nameof(CharInfoExperienceVM.LevelUp))]
        [HarmonyPrefix]
        public static bool CharInfoExperienceVM_LevelUp_Prefix(CharInfoExperienceVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.RequestLevelingUI(__instance.Unit.Value.Unit.UniqueId);
            return canContinue;
        }
    }
}
