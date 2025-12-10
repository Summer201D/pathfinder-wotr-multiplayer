using HarmonyLib;
using Kingmaker.UI.MVVM._VM.Party;
using WOTRMultiplayer.MP.Entities.Leveling;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class PartyCharacterVMPatches
    {
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
    }
}
