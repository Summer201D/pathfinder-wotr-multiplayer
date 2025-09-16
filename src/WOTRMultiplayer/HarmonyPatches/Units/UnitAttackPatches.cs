using System;
using System.Linq;
using HarmonyLib;
using Kingmaker.TurnBasedMode;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.MP.Entities;
using WOTRMultiplayer.MP.Entities.Combat;

namespace WOTRMultiplayer.HarmonyPatches.Units
{
    [HarmonyPatch]
    public class UnitAttackPatches
    {
        [HarmonyPatch(typeof(UnitCommands), nameof(UnitCommands.Run), [typeof(UnitCommand)])]
        [HarmonyPrefix]
        public static bool UnitCommands_Run_Prefix(UnitCommand cmd)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            return cmd is not UnitAttack || cmd.CreatedByPlayer || cmd.Executor == null;
        }

        [HarmonyPatch(typeof(UnitAttack), nameof(UnitAttack.OnStart))]
        [HarmonyPostfix]
        public static void UnitAttack_OnStart_Postfix(UnitAttack __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            OnUnitAttack(__instance);
        }

        [HarmonyPatch(typeof(UnitCommand), nameof(UnitCommand.Interrupt))]
        [HarmonyPrefix]
        public static void UnitAttack_OnStart_Prefix(UnitCommand __instance)
        {
            if (!Main.Multiplayer.IsActive || __instance is not UnitAttack attack || !attack.CreatedByPlayer)
            {
                return;
            }

            Main.GetLogger<UnitAttackPatches>().LogWarning("Interrupting attack command. AttackIndex={AttackIndex}, AttackCount={AttackCount}, StackTrace={StackTrace}", attack.m_AttackIndex, attack.m_AllAttacks.Count, Environment.StackTrace);
        }

        private static void OnUnitAttack(UnitAttack command)
        {
            var path = PathVisualizer.Instance.CurrentPathForUnit(command.Executor.View);
            var networkPath = path?.vectorPath.Select(v => new NetworkVector3(v.x, v.y, v.z)).ToList();
            var networkAbility = new NetworkUnitAttack
            {
                ExecutorUnitId = command.Executor.UniqueId,
                TargetUnitId = command.TargetUnit?.UniqueId,
                IsFullAttack = command.IsAttackFull,
                VectorPath = networkPath
            };

            Main.Multiplayer.OnUnitAttack(networkAbility);
        }
    }
}
