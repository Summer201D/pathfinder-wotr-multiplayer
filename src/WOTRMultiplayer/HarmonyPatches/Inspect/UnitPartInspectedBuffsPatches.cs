using HarmonyLib;
using Kingmaker.UnitLogic.Parts;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Inspect
{
    [HarmonyPatch]
    public class UnitPartInspectedBuffsPatches
    {
        [HarmonyPatch(typeof(UnitPartInspectedBuffs), nameof(UnitPartInspectedBuffs.MakeCheck))]
        [HarmonyPrefix]
        public static bool UnitPartInspectedBuffs_MakeCheck_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            Main.GetLogger<UnitPartInspectedBuffsPatches>().LogError("TODO");
            return false;
        }
    }
}
