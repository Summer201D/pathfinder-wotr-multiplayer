using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Microsoft.Extensions.Logging;
using static Kingmaker.RuleSystem.RulebookEvent;

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

        [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.OnTrigger))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleAttackRoll_OnTrigger_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var attr = MethodBase.GetCurrentMethod().GetCustomAttribute<HarmonyPatch>();
            var target = $"{attr.info.declaringType.Name}.{attr.info.methodName}";
            var matcher = new CodeMatcher(instructions);

            ReplaceD20Generator(matcher, target);

            return matcher.Instructions();
        }

        private static void ReplaceD20Generator(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RuleRollDicePatches), nameof(RuleRollDicePatches.GenerateReplaceableG20));
            var lookFor = AccessTools.Method(typeof(Dice), nameof(Dice.GenerateD20));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match == null)
            {
                Main.GetLogger<HarmonyTranspiler>().LogError("Transpiler has not been applied. Target={target}", target);
                return;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
                {
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Call, replaceWith)
                };

            match.Insert(newInstructions);
            Main.GetLogger<HarmonyTranspiler>().LogInformation("Transpiler has been applied. Target={target}", target);
        }

        public static RuleRollD20 GenerateReplaceableG20(bool isFake, RuleAttackRoll ruleAttackRoll)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Dice.GenerateD20(isFake);
            }

            var shouldRunOriginalLogic = Main.Multiplayer.OnBeforeRuleAttackRoleTrigger(ruleAttackRoll);
            if (shouldRunOriginalLogic)
            {
                var roll = Dice.GenerateD20(isFake);
                Main.Multiplayer.OnAfterRuleAttackRoleTrigger(ruleAttackRoll, roll);
                return roll;
            }

            return ruleAttackRoll.D20;
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
