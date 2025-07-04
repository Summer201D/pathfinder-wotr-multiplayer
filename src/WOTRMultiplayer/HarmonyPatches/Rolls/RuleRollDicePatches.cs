using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleRollDicePatches
    {
        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.OnTrigger))]
        [HarmonyPostfix]
        public static void RuleRollDice_OnTrigger_Postfix(RuleRollDice __instance, RulebookEventContext context)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnAfterRuleRollDiceTrigger(__instance);
        }

        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.OnTrigger))]
        [HarmonyPrefix]
        public static bool RuleRollDice_OnTrigger_Prefix(RuleRollDice __instance, RulebookEventContext context)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var shouldRunOriginal = Main.Multiplayer.OnBeforeRuleRollDiceTrigger(__instance);
            return shouldRunOriginal;
        }
    }
}
