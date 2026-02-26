using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class RuleDealStatDamagePatches
    {
        [HarmonyPatch(typeof(RuleDealStatDamage), nameof(RuleDealStatDamage.OnTrigger))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleDealStatDamage_OnTrigger_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var replaceWith = AccessTools.Method(typeof(RuleDealStatDamagePatches), nameof(RuleDealStatDamagePatches.RollDamage));
            var lookFor = AccessTools.Method(typeof(RuleDealStatDamage), nameof(RuleDealStatDamage.RollDamage));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<RuleDealStatDamagePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };

            match = match.RemoveInstruction().Insert(newInstructions);
            Main.GetLogger<RuleDealStatDamagePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        /// <summary>
        /// copy-paste of Kingmaker.RuleSystem.Rules.RuleDealStatDamage.RollDamage with extra roll hooks, logic is the same
        /// </summary>
        /// <param name="damageFormula"></param>
        /// <param name="criticalModifier"></param>
        /// <param name="maximized"></param>
        /// <param name="ruleDealStatDamage"></param>
        /// <returns></returns>
        private static int RollDamage(DiceFormula damageFormula, int criticalModifier, bool maximized, RuleDealStatDamage ruleDealStatDamage)
        {
            try
            {
                if (damageFormula == DiceFormula.Zero)
                {
                    return 0;
                }
                if (damageFormula == DiceFormula.One)
                {
                    return 1;
                }
                if (maximized)
                {
                    return damageFormula.Rolls * damageFormula.Dice.Sides() * criticalModifier;
                }

                if (Main.Multiplayer.IsActive)
                {
                    var rollOverride = Main.Rolls.OnBeforeRuleDealStatDamageRoll(ruleDealStatDamage, criticalModifier);
                    if (rollOverride.HasValue)
                    {
                        return rollOverride.Value;
                    }
                }

                var damage = RollCriticalDamage(damageFormula, criticalModifier);

                if (Main.Multiplayer.IsActive)
                {
                    var damageRoll = RuleRollD100.FromInt(ruleDealStatDamage.Initiator, damage);
                    damageRoll.m_Result = damage;
                    Main.Rolls.OnAfterRuleDealStatDamageRoll(ruleDealStatDamage, damageRoll, criticalModifier);
                }

                return damage;
            }
            catch (Exception ex)
            {
                Main.GetLogger<RuleDealStatDamagePatches>().LogError(ex, "Error while calculating stat damage");
                throw;
            }
        }

        private static int RollCriticalDamage(DiceFormula damageFormula, int criticalModifier)
        {
            int num = 0;
            while (criticalModifier-- > 0)
            {
                num += RulebookEvent.Dice.D(damageFormula);
            }

            return num;
        }
    }
}
