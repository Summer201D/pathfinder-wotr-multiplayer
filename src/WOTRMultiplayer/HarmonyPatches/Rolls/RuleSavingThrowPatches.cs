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
    public class RuleSavingThrowPatches
    {
        [HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.RollD20))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RuleSavingThrow_RollD20_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var replaceWith = AccessTools.Method(typeof(RuleSavingThrowPatches), nameof(RuleSavingThrowPatches.SavingThrowRollD20));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.Start();
            if (match.Instruction.opcode != OpCodes.Ldarg_0)
            {
                Main.GetLogger<RuleSavingThrowPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match = match.RemoveInstructions(3).Insert(newInstructions);

            Main.GetLogger<RuleSavingThrowPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static RuleRollD20 SavingThrowRollD20(RuleSavingThrow savingThrow)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Rulebook.Trigger<RuleRollD20>(savingThrow.D20);
            }

            var shouldContinue = Main.Rolls.OnBeforeRuleSavingThrowRoll(savingThrow);
            if (!shouldContinue)
            {
                return savingThrow.D20;
            }

            var d20 = Rulebook.Trigger<RuleRollD20>(savingThrow.D20);
            Main.Rolls.OnAfterRuleSavingThrowTrigger(savingThrow);
            return d20;
        }
    }
}
