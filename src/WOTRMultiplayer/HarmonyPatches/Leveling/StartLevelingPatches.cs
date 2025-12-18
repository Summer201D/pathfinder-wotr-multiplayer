using HarmonyLib;
using Kingmaker;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UI.MVVM._VM.Party;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.LevelClassScores.Experience;
using WOTRMultiplayer.MP.Entities.Leveling;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class StartLevelingPatches
    {
        [HarmonyPatch(typeof(CharInfoExperienceVM), nameof(CharInfoExperienceVM.LevelUp))]
        [HarmonyPrefix]
        public static bool CharInfoExperienceVM_LevelUp_Prefix(CharInfoExperienceVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.RequestLevelingUI(__instance.Unit.Value.Unit.UniqueId, MP.Entities.Leveling.NetworkLevelingType.Leveling);
            return canContinue;
        }

        [HarmonyPatch(typeof(PartyCharacterVM), nameof(PartyCharacterVM.LevelUp))]
        [HarmonyPrefix]
        public static bool PartyCharacterVM_LevelUp_Prefix(PartyCharacterVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.RequestLevelingUI(__instance.UnitEntityData.UniqueId, NetworkLevelingType.Leveling);
            return canContinue;
        }

        [HarmonyPatch(typeof(PartyCharacterVM), nameof(PartyCharacterVM.MythicLevelUp))]
        [HarmonyPrefix]
        public static bool PartyCharacterVM_MythicLevelUp_Prefix(PartyCharacterVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.RequestLevelingUI(__instance.UnitEntityData.UniqueId, NetworkLevelingType.MythicLeveling);
            return canContinue;
        }

        [HarmonyPatch(typeof(OpenSelectMythicUI), nameof(OpenSelectMythicUI.RunAction))]
        [HarmonyPrefix]
        public static void OpenSelectMythicUI_RunAction_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var unitId = Game.Instance.Player.MainCharacter.Value.UniqueId;
            Main.Multiplayer.ForceLevelingUI(unitId, NetworkLevelingType.MythicLeveling);
        }
    }
}
