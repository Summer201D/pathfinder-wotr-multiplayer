using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Armies.TacticalCombat.Controllers;
using Kingmaker.Controllers.Clicks.Handlers;

namespace WOTRMultiplayer.HarmonyPatches.Combat.Crusades
{
    [HarmonyPatch]
    public class TacticalCombatClickHandlersPatches
    {
        [HarmonyPatch(typeof(TacticalCombatClickGroundHandler), nameof(TacticalCombatClickGroundHandler.OnClick))]
        [HarmonyPrefix]
        public static bool TacticalCombatClickGroundHandler_OnClick_Prefix(ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canControl = Main.Multiplayer.CanControlTacticalCombat();
            if (!canControl)
            {
                __result = false;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(ClickWithSelectedAbilityHandler), nameof(ClickWithSelectedAbilityHandler.OnClick))]
        [HarmonyPrefix]
        public static bool ClickWithSelectedAbilityHandler_OnClick_Prefix(ref bool __result)
        {
            if (!Main.Multiplayer.IsActive || !TacticalCombatHelper.IsActive)
            {
                return true;
            }

            var canControl = Main.Multiplayer.CanControlTacticalCombat();
            if (!canControl)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
