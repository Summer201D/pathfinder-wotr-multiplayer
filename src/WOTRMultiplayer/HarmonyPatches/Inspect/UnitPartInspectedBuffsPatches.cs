using HarmonyLib;
using Kingmaker.UnitLogic.Parts;

namespace WOTRMultiplayer.HarmonyPatches.Inspect
{
    [HarmonyPatch]
    public class UnitPartInspectedBuffsPatches
    {
        [HarmonyPatch(typeof(UnitPartInspectedBuffs), nameof(UnitPartInspectedBuffs.MakeCheck))]
        [HarmonyPrefix]
        public static bool UnitPartInspectedBuffs_MakeCheck_Prefix(UnitPartInspectedBuffs __instance, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            __result = true;
            return false;
        }
    }
}
