using HarmonyLib;
using Kingmaker.View.MapObjects;
using WOTRMultiplayer.Entities.MapObjects;

namespace WOTRMultiplayer.HarmonyPatches.MapObjects
{
    [HarmonyPatch]
    public class InteractionLootPartPatches
    {
        [HarmonyPatch(typeof(InteractionLootPart), nameof(InteractionLootPart.OnLootClosed))]
        [HarmonyPrefix]
        public static void InteractionLootPart_OnLootClosed_Prefix(InteractionLootPart __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var mapObject = Main.Mapper.Map<NetworkMapObject>(__instance.Owner);
            Main.Multiplayer.OnLootClosed(mapObject);
        }
    }
}
