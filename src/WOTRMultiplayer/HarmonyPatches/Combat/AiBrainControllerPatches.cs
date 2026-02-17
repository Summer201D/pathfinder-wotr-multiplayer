using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Kingmaker.AI;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Pathfinding;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Commands.Base;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.GameInteraction.CombatLog;
using WOTRMultiplayer.Entities.Combat;
using WOTRMultiplayer.Extensions;
using WOTRMultiplayer.Services.Random;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class AiBrainControllerPatches
    {
        [HarmonyPatch(typeof(CombatAiData), nameof(CombatAiData.UseAction))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CombatAiData_UseAction_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var lookFor = AccessTools.Method(typeof(RulebookEvent.Dice), nameof(RulebookEvent.Dice.D), [typeof(DiceFormula)]);
            var replaceWith = AccessTools.Method(typeof(AiBrainControllerPatches), nameof(AiBrainControllerPatches.RollAIActionCooldown));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<AiBrainControllerPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstruction().Insert(newInstructions);
            Main.GetLogger<AiBrainControllerPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);

            return matcher.Instructions();
        }

        private static int RollAIActionCooldown(DiceFormula diceFormula, AiAction aiAction, UnitCommand unitCommand)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return RulebookEvent.Dice.D(diceFormula);
            }

            try
            {
                var sessionSeed = Main.Multiplayer.GetSessionSeed();
                var combatSeed = Main.Multiplayer.GetCombatSeed();
                var combatTurnSeed = Main.Multiplayer.GetCombatTurnSeed();
                var identifier = $"{nameof(CombatAiData)}:{nameof(CombatAiData.UseAction)}:{nameof(RollAIActionCooldown)}:{Game.Instance.Player.GameId}:{unitCommand.Executor.UniqueId}:{aiAction.Blueprint.AssetGuid}:{aiAction.Blueprint.name}:{unitCommand.TargetUnit?.UniqueId}:{sessionSeed}:{combatSeed}:{combatTurnSeed}";
                var random = Main.Multiplayer.ValueGenerator.GetRandom(IdentifierLifetime.CombatTurn, identifier);
                var cooldown = diceFormula.Roll(random);
                Main.GetLogger<AiBrainControllerPatches>().LogInformation("AI action cooldown has been rolled. Cooldown={Cooldown}, Identifier={Identifier}", cooldown, identifier);
                return cooldown;
            }
            catch (Exception ex)
            {
                Main.GetLogger<AiBrainControllerPatches>().LogError(ex, "Unable to roll AI action cooldown. UnitId={UnitId}", unitCommand.Executor?.UniqueId);
                throw;
            }
        }

        [HarmonyPatch(typeof(AiBrainController), nameof(AiBrainController.FindBestAction))]
        [HarmonyPostfix]
        public static void AiBrainController_FindBestAction_Postfix(UnitEntityData unit, DecisionContext context, ref AiAction bestActionResult, ref UnitEntityData bestTargetResult)
        {
            if (!Main.Multiplayer.IsActive || bestActionResult == null)
            {
                return;
            }

            var action = new NetworkAIAction
            {
                Id = bestActionResult.Blueprint.AssetGuid.ToString(),
                Name = bestActionResult.Blueprint.name,
                ActionType = $"{bestActionResult.GetType().Name}_{bestActionResult.Blueprint.GetType().Name}",
                UnitId = unit.UniqueId,
                TargetId = bestTargetResult?.UniqueId,
                DecisionContext = new NetworkAIDecisionContext
                {
                    BestEnableFiveFootStep = context.BestEnableFiveFootStep,
                    VectorPath = context.BestPath?.vectorPath.Select(v => v.ToNetworkVector3()).ToList() ?? [],
                    BestDestinationPoint = context.BestDestinationPoint.ToNetworkVector3(),
                }
            };

            var possibleOverride = Main.Multiplayer.OnAfterAISelectedAction(action);
            if (possibleOverride == null)
            {
                return;
            }

            if (possibleOverride.TargetId != bestTargetResult.UniqueId)
            {
                Main.PlayerNotification.AddCombatText(WellKnownKeys.GameNotifications.Combat.ActionOverride.Key, CombatTextSeverity.Debug, new UnitEntityLog(unit.UniqueId), possibleOverride.Name, new UnitEntityLog(bestTargetResult.UniqueId), new UnitEntityLog(possibleOverride.TargetId));
                Main.GetLogger<AiBrainControllerPatches>().LogWarning("AI action target has been overridden. UnitId={UnitId}, ActionId={ActionId}, ActionName={ActionName}, PreviousTarget={PreviousTarget}, NewTarget={NewTarget}", unit.UniqueId, possibleOverride.Id, possibleOverride.Name, bestTargetResult.UniqueId, possibleOverride.TargetId);
                bestTargetResult = Main.State.GetUnitEntity(possibleOverride.TargetId);
                var path = new ForcedPath([.. possibleOverride.DecisionContext.VectorPath.Select(x => x.ToUnityVector3())]);
                context.BestPath = path;
            }
        }
    }
}
