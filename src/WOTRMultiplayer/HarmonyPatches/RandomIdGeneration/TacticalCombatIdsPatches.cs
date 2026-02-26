using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Armies.State;
using Kingmaker.Armies.TacticalCombat.Controllers;
using Kingmaker.Blueprints;
using Kingmaker.Globalmap.State;
using Kingmaker.Kingdom.Blueprints;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Services.Random;

namespace WOTRMultiplayer.HarmonyPatches.RandomIdGeneration
{
    [HarmonyPatch]
    public class TacticalCombatIdsPatches
    {
        [HarmonyPatch(typeof(TacticalCombatController), nameof(TacticalCombatController.CreateUnit))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TacticalCombatController_CreateUnit_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.Method(typeof(Guid), nameof(Guid.NewGuid));
            var replaceWith = AccessTools.Method(typeof(TacticalCombatIdsPatches), nameof(TacticalCombatIdsPatches.CreateTacticalCombatUnitId));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<TacticalCombatIdsPatches>().LogError("Unable to find Guid.NewGuid() call. Target={Target}", target);
                return matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>
            {
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldarg_S, 4),
                new(OpCodes.Ldloc_1),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstructions(5).Insert(newInstructions);
            Main.GetLogger<TacticalCombatIdsPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(TacticalCombatController), nameof(TacticalCombatController.SetupLeader))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TacticalCombatController_SetupLeader_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.Method(typeof(Guid), nameof(Guid.NewGuid));
            var replaceWith = AccessTools.Method(typeof(TacticalCombatIdsPatches), nameof(TacticalCombatIdsPatches.CreateTacticalCombatLeaderId));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<TacticalCombatIdsPatches>().LogError("Unable to find Guid.NewGuid() call. Target={Target}", target);
                return matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>
            {
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Ldarg_S, 4),
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstructions(5).Insert(newInstructions);
            Main.GetLogger<TacticalCombatIdsPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static string CreateTacticalCombatLeaderId(GlobalMapArmyState globalMapArmyState, bool attacker, BlueprintFaction faction)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Guid.NewGuid().ToString();
            }

            try
            {
                var crusadeArmyCombatSeed = Game.Instance.TacticalCombat.Data.Seed;
                var leader = globalMapArmyState.Data.Leader;
                var armyName = globalMapArmyState.Data.ArmyName;
                var rawIdentifier = $"{CommonTranspilerReplacements.GetSharedIdentifierPart()}:{globalMapArmyState.Id}:{globalMapArmyState.ArmyType}:{armyName?.ArmyName}:{armyName?.ArmyIndex}:{attacker}:{leader?.Blueprint.name}:{faction.name}:{crusadeArmyCombatSeed}";
                var id = Main.Multiplayer.ValueGenerator.CreateGuid(IdentifierLifetime.Area, rawIdentifier);
                Main.GetLogger<TacticalCombatIdsPatches>().LogInformation("Leader UnitId has been generated. Id={Id}, Seed={Seed}, RawIdentifier={RawIdentifier}", id, crusadeArmyCombatSeed, rawIdentifier);
                return id.ToString();
            }
            catch (Exception ex)
            {
                Main.GetLogger<TacticalCombatIdsPatches>().LogError(ex, "Error while generating new army leader unit Id");
                throw;
            }
        }

        private static string CreateTacticalCombatUnitId(GlobalMapArmyState globalMapArmyState, SquadState squadState, RegionId regionId, BlueprintFaction faction)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Guid.NewGuid().ToString();
            }

            try
            {
                var crusadeArmyCombatSeed = Game.Instance.TacticalCombat.Data.Seed;
                var leader = globalMapArmyState.Data.Leader;
                var armyName = globalMapArmyState.Data.ArmyName;
                var rawIdentifier = $"{CommonTranspilerReplacements.GetSharedIdentifierPart()}:{globalMapArmyState.Id}:{globalMapArmyState.ArmyType}:{armyName?.ArmyName}:{armyName?.ArmyIndex}:{squadState.Size}:{squadState.Unit.name}:{regionId}:{leader?.Blueprint.name}:{faction.name}:{crusadeArmyCombatSeed}";
                var id = Main.Multiplayer.ValueGenerator.CreateGuid(IdentifierLifetime.Area, rawIdentifier);
                Main.GetLogger<TacticalCombatIdsPatches>().LogInformation("Army UnitId has been generated. Id={Id}, Seed={Seed}, RawIdentifier={RawIdentifier}", id, crusadeArmyCombatSeed, rawIdentifier);
                return id.ToString();
            }
            catch (Exception ex)
            {
                Main.GetLogger<TacticalCombatIdsPatches>().LogError(ex, "Error while generating new army unit Id");
                throw;
            }
        }
    }
}
