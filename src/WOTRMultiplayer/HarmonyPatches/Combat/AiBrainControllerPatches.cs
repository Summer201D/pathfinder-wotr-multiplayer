using HarmonyLib;
using Kingmaker;
using Kingmaker.AI;
using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class AiBrainControllerPatches
    {
        [HarmonyPatch(typeof(AiBrainController), nameof(AiBrainController.TickBrain))]
        [HarmonyPrefix]
        public static bool AiBrainController_TickBrain_Prefix(AiBrainController __instance, UnitEntityData unit)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = !Main.Multiplayer.IsControlledByPlayers(unit.UniqueId);
            if (!canContinue && Game.Instance.TurnBasedCombatController.CurrentTurn != null)
            {
                // game treats characters without control as AI and tries to skip turn if they are stuck
                // but in reality those characters are controlled by other players and we are waiting for their actions
                // I believe this could reworked by using transpiler to replace generic condition 'IsDirectlyControllable' in TurnController.Tick => (Status == TurnStatus.Acting && Rider.Commands.Empty && !Rider.IsDirectlyControllable)
                // with something smarter, but resetting counters work fine for now
                Game.Instance.TurnBasedCombatController.CurrentTurn.AIForcedTickCount = 0;
                Game.Instance.TurnBasedCombatController.CurrentTurn.FramesWaitedForStuckAI = 0;
                Game.Instance.TurnBasedCombatController.CurrentTurn.TimeWaitedForIdleAI = 0;
            }

            return canContinue;
        }
    }
}
