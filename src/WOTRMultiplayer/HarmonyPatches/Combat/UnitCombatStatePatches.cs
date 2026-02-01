using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class UnitCombatStatePatches
    {
        [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.AttackOfOpportunity))]
        [HarmonyPostfix]
        public static void UnitCombatState_AttackOfOpportunity_Postfix(UnitCombatState __instance, UnitEntityData target, bool tricksterAttack, bool simulate, bool disengaging, bool afterDefensivelyFail, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive || simulate)
            {
                return;
            }

            Main.GetLogger<UnitCombatStatePatches>().LogWarning("AoO: UnitId={UnitId}, TargetUnitId={TargetUnitId}, Result={Result}, TricksterAttack={TricksterAttack}, Disengaging={Disengaging}, AfterDefensivelyFail={AfterDefensivelyFail}, UnitPreventAoO={UnitPreventAoO}, TargetPreventAoO={TargetPreventAoO}",
                __instance.Unit.UniqueId, target?.UniqueId, __result, tricksterAttack, disengaging, afterDefensivelyFail, __instance.PreventAttacksOfOpporunityNextFrame, target.CombatState.PreventAttacksOfOpporunityNextFrame);
        }

        [HarmonyPatch(typeof(UnitCombatEngagementController), nameof(UnitCombatEngagementController.ProvokeAttackOfOpportunityAfterDefensivelyFail))]
        [HarmonyPrefix]
        public static void UnitCombatEngagementController_ProvokeAttackOfOpportunityAfterDefensivelyFail_Prefix(UnitCombatEngagementController __instance, UnitEntityData unit)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.GetLogger<UnitCombatStatePatches>().LogWarning("AoO (Def Casting failed): UnitId={UnitId}, DefQueue={UnitsInQueue}, RangedQueue={RangedQueue}", unit.UniqueId, __instance.m_UnitsProvokeAttackOfOpportunityAfterDefensivelyFail.Count, __instance.m_UnitsProvokeAttackOfOpportunity.Count);
        }

        [HarmonyPatch(typeof(UnitCombatEngagementController), nameof(UnitCombatEngagementController.ProvokeAttackOfOpportunity))]
        [HarmonyPrefix]
        public static void UnitCombatEngagementController_ProvokeAttackOfOpportunity_Prefix(UnitCombatEngagementController __instance, UnitEntityData unit)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.GetLogger<UnitCombatStatePatches>().LogWarning("AoO (Ranged Attack): UnitId={UnitId}, DefQueue={UnitsInQueue}, RangedQueue={RangedQueue}", unit.UniqueId, __instance.m_UnitsProvokeAttackOfOpportunityAfterDefensivelyFail.Count, __instance.m_UnitsProvokeAttackOfOpportunity.Count);
        }

        [HarmonyPatch(typeof(UnitUseAbility), nameof(UnitUseAbility.OnTick))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> UnitUseAbility_OnTick_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var replaceWith = AccessTools.Method(typeof(UnitCombatStatePatches), nameof(UnitCombatStatePatches.GetDefensiveCastingRollDelay));
            var lookFor = AccessTools.PropertyGetter(typeof(UnitCommand), nameof(UnitCommand.IsPretendAct));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<UnitCombatStatePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            match = match.Advance(-2);
            var labels = match.Instruction.ExtractLabels();
            var newInstructions = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0).WithLabels(labels),
                new(OpCodes.Call, replaceWith),
            };
            match = match.RemoveInstructions(9).Insert(newInstructions);

            Main.GetLogger<UnitCombatStatePatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        /// <summary>
        /// failed defensive casting is one of the AoO (Attack of Opportunity) reasons in the game, but unlike others, this one is time-based.
        /// further more, its roll is always ignored by another Range/RangeTouch AoO logic (frame-based).
        /// basically, by casting Range Touch ability, you will either roll or not roll defensive casting, but it's still going to be ignored either way due to Range/RangeTouch AoO superiority.
        /// this leads to shitty situations where some players roll it while others don't, yet it's still ignored for everyone.
        /// reducing delay to 0 in combat removes time entropy, so defensive casting is always rolled for everyone, but actual usage is still processed by default AoO logic
        /// also, it shouldn't produce any extra AoO since turn-based combat has no way to cancel commands.
        ///
        /// as another option: skip first 1..n frames, allowing the game to cancel command without producing defensive casting AoO
        /// </summary>
        /// <param name="unitUseAbility"></param>
        /// <returns></returns>
        private static float GetDefensiveCastingRollDelay(UnitUseAbility unitUseAbility)
        {
            var minDelay = 1f;
            if (Main.Multiplayer.IsActive && Main.Multiplayer.IsInCombat && !unitUseAbility.IsPretendAct)
            {
                minDelay = 0f;
            }

            var maxDelay = unitUseAbility.IsPretendAct ? unitUseAbility.PretendActTime : float.MaxValue;
            var delay = Math.Min(minDelay, maxDelay);
            return delay;
        }
    }
}
