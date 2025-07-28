using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

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

        [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.OnTrigger))]
        [HarmonyPostfix]
        public static void RuleCalculateDamage_OnTrigger_Postfix(RuleCalculateDamage __instance, RulebookEventContext context)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnAfterRuleCalculateDamageTrigger(__instance);
        }

        [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.OnTrigger))]
        [HarmonyPrefix]
        public static bool RuleCalculateDamage_OnTrigger_Prefix(RuleCalculateDamage __instance, RulebookEventContext context)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var shouldRunOriginal = Main.Multiplayer.OnBeforeRuleCalculateDamageTrigger(__instance);
            return shouldRunOriginal;
        }


        //[HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.OnTrigger))]
        //[HarmonyPostfix]
        //public static void RuleSavingThrow_OnTrigger_Postfix(RuleSavingThrow __instance, RulebookEventContext context)
        //{
        //    if (!Main.Multiplayer.IsActive)
        //    {
        //        return;
        //    }

        //    Main.Multiplayer.OnAfterRuleSavingThrowTrigger(__instance);
        //}

        //[HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.OnTrigger))]
        //[HarmonyPrefix]
        //public static bool RuleSavingThrow_OnTrigger_Prefix(RuleSavingThrow __instance, RulebookEventContext context)
        //{
        //    if (!Main.Multiplayer.IsActive)
        //    {
        //        return true;
        //    }

        //    var shouldRunOriginal = Main.Multiplayer.OnBeforeRuleSavingThrowTrigger(__instance);
        //    return shouldRunOriginal;
        //}

        [HarmonyPatch(typeof(RuleHealDamage), nameof(RuleHealDamage.Roll))]
        [HarmonyPostfix]
        public static void RuleHealDamage_Roll_Postfix(RuleHealDamage __instance, ref int __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __result = Main.Multiplayer.OnAfterRollRuleHealDamage(__instance, __result);
        }
    }

    // RuleStatCheck
    // RuleSkillCheck
    // RuleSpellResistance
    // RuleAttackRoll
    // RuleSavingThrow
    // RuleAttackWithWeapon


    // RuleCalculateDamage
    // RuleHealDamage
}
