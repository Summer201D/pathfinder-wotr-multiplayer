using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem;
using Microsoft.Extensions.Logging;
using Owlcat.Runtime.Core.Math;
using WOTRMultiplayer.Extensions;

namespace WOTRMultiplayer.HarmonyPatches.Rolls
{
    [HarmonyPatch]
    public class TacticalCombatHelperRollsPatches
    {
        [HarmonyPatch(typeof(TacticalCombatHelper), nameof(TacticalCombatHelper.GetDiceResult))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TacticalCombatHelper_GetDiceResult_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(TacticalCombatHelperRollsPatches), nameof(TacticalCombatHelperRollsPatches.Roll));
            var lookFor = AccessTools.Method(typeof(RulebookEvent.Dice), nameof(RulebookEvent.Dice.D), [typeof(DiceFormula)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<TacticalCombatHelperRollsPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };

            match = match.RemoveInstructions(1).Insert(newInstructions);
            Main.GetLogger<TacticalCombatHelperRollsPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(ProbabilityCurveSampler), nameof(ProbabilityCurveSampler.SampleRandom))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ProbabilityCurveSampler_SampleRandom_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(TacticalCombatHelperRollsPatches), nameof(TacticalCombatHelperRollsPatches.RollSample));
            var lookFor = AccessTools.PropertyGetter(typeof(UnityEngine.Random), nameof(UnityEngine.Random.value));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<TacticalCombatHelperRollsPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };

            match = match.RemoveInstructions(1).Insert(newInstructions);
            Main.GetLogger<TacticalCombatHelperRollsPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static float RollSample()
        {
            // it's only used for tactical combat, but just in case
            if (!Main.Multiplayer.IsActive || !TacticalCombatHelper.IsActive)
            {
                return UnityEngine.Random.value;
            }

            try
            {
                var targetEvent = Rulebook.CurrentContext.CurrentEvent as RulebookTargetEvent;
                if (targetEvent != null
                    && (PatchesUtils.IsHelperUnit(targetEvent.Initiator?.UniqueId) || PatchesUtils.IsHelperUnit(targetEvent.Target?.UniqueId)))
                {
                    return UnityEngine.Random.value;
                }

                var turnInfo = Game.Instance.TacticalCombat?.Data?.Turn;
                var turnNumber = turnInfo?.Number ?? -1;
                var unitTurn = turnInfo?.Unit.UniqueId;
                var seededContext = Main.Multiplayer.GetSeededContext();
                var identifier = $"{nameof(ProbabilityCurveSampler)}:{nameof(RollSample)}:{unitTurn}:{turnNumber}:{targetEvent?.Initiator?.UniqueId}:{targetEvent?.Target?.UniqueId}_{seededContext.Id}";
                var random = Main.Multiplayer.ValueGenerator.GetRandom(seededContext.Lifetime, identifier);
                var sample = random.NextFloat(0f, 1f);
                Main.GetLogger<TacticalCombatHelperRollsPatches>().LogInformation("Tactical combat dice sample has been rolled. Sample={Sample}, Identifier={Identifier}", sample, identifier);
                return sample;
            }
            catch (Exception ex)
            {
                Main.GetLogger<TacticalCombatHelperRollsPatches>().LogError(ex, "Error while rolling tactical combat dice sample");
                throw;
            }
        }

        private static int Roll(DiceFormula diceFormula)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return RulebookEvent.Dice.D(diceFormula);
            }

            try
            {
                var targetEvent = Rulebook.CurrentContext.CurrentEvent as RulebookTargetEvent;
                if (targetEvent != null
                    && (PatchesUtils.IsHelperUnit(targetEvent.Initiator?.UniqueId) || PatchesUtils.IsHelperUnit(targetEvent.Target?.UniqueId)))
                {
                    return RulebookEvent.Dice.D(diceFormula);
                }

                var turnInfo = Game.Instance.TacticalCombat?.Data?.Turn;
                var turnNumber = turnInfo?.Number ?? -1;
                var unitTurn = turnInfo?.Unit.UniqueId;
                var seededContext = Main.Multiplayer.GetSeededContext();
                var identifier = $"{nameof(TacticalCombatHelper)}:{nameof(Roll)}:{unitTurn}:{turnNumber}:{targetEvent?.Initiator?.UniqueId}:{targetEvent?.Target?.UniqueId}:{diceFormula.Rolls}:{diceFormula.Dice}_{seededContext.Id}";
                var random = Main.Multiplayer.ValueGenerator.GetRandom(seededContext.Lifetime, identifier);
                var result = diceFormula.Roll(random);
                Main.GetLogger<TacticalCombatHelperRollsPatches>().LogInformation("Tactical combat dice has been rolled. Result={Result}, Identifier={Identifier}", result, identifier);
                return result;
            }
            catch (Exception ex)
            {
                Main.GetLogger<TacticalCombatHelperRollsPatches>().LogError(ex, "Error while rolling tactical combat dice result");
                throw;
            }
        }
    }
}
