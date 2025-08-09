using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RandomEncounters;
using Kingmaker.RandomEncounters.Settings;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Rest
{
    [HarmonyPatch]
    public class RestRandomEncounterContextPatches
    {
        [HarmonyPatch(typeof(RestController), nameof(RestController.TryRollRandomEncounter))]
        [HarmonyPrefix]
        public static void RestController_TryRollRandomEncounter_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }


            Main.Multiplayer.OnBeforeTryRollRandomEncounter();
        }

        [HarmonyPatch(typeof(RestController), nameof(RestController.TryRollRandomEncounter))]
        [HarmonyPostfix]
        public static void RestController_TryRollRandomEncounter_Postfix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }


            Main.Multiplayer.OnAfterTryRollRandomEncounter();
        }

        [HarmonyPatch(typeof(RestController), nameof(RestController.TryRollSpecialEncounter))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RestController_TryRollSpecialEncounter_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!ReplaceSpecialEncounterRoll(matcher, target) || !ReplaceTimePassedEncounterRoll(matcher, target))
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Failed to apply all replacements. Target={target}", target);
                return instructions;
            }

            Main.GetLogger<RestRandomEncounterContextPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
            return matcher.Instructions();
        }

        private static bool ReplaceSpecialEncounterRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnSpecialEncounterRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(int), typeof(int)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldloc_1),
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        private static bool ReplaceTimePassedEncounterRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnSpecialEncounterHoursPassedRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        public static int OnSpecialEncounterRoll(int randomMin, int randomMax, BlueprintCampingEncounter encounter)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            var encounterKey = encounter.AssetGuid.ToString();
            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.SpecialEncounters.TryAdd(encounterKey, roll);
                return roll;
            }

            context.Encounter.SpecialEncounters.TryGetValue(encounterKey, out var remoteRoll);
            return remoteRoll;
        }

        public static float OnSpecialEncounterHoursPassedRoll(float randomMin, float randomMax)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            if (context.IsRecording)
            {
                context.Encounter.HoursPassedBeforeEncounter = UnityEngine.Random.Range(randomMin, randomMax);
                return context.Encounter.HoursPassedBeforeEncounter;
            }

            return context.Encounter.HoursPassedBeforeEncounter;
        }

        [HarmonyPatch(typeof(RestController), nameof(RestController.RollEncounter))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RestController_RollEncounter_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!ReplaceEncounterGuardSlotRoll(matcher, target) || !ReplaceEncounterCamouflageRoll(matcher, target))
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Failed to apply all replacements. Target={target}", target);
                return instructions;
            }

            Main.GetLogger<RestRandomEncounterContextPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
            return matcher.Instructions();
        }

        private static bool ReplaceEncounterGuardSlotRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterGuardSlotRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(int), typeof(int)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        private static bool ReplaceEncounterCamouflageRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterCamouflageRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(int), typeof(int)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        public static int OnEncounterGuardSlotRoll(int randomMin, int randomMax)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.GuardSlotRoll = roll;
                return roll;
            }

            return context.Encounter.GuardSlotRoll;
        }

        public static int OnEncounterCamouflageRoll(int randomMin, int randomMax)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.CamouflageRoll = roll;
                return roll;
            }

            return context.Encounter.CamouflageRoll;
        }

        [HarmonyPatch(typeof(RandomEncounterUnitSelector), nameof(RandomEncounterUnitSelector.SelectUnits))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RandomEncounterUnitSelector_SelectUnits_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterRandomUnitSeedRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(int), typeof(int)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return instructions;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            Main.GetLogger<RestRandomEncounterContextPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
            return matcher.Instructions();
        }

        public static int OnEncounterRandomUnitSeedRoll(int randomMin, int randomMax)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.RandomUnitSeed = roll;
                return roll;
            }

            return context.Encounter.RandomUnitSeed;
        }

        [HarmonyPatch(typeof(RandomEncounterUnitSelector), nameof(RandomEncounterUnitSelector.PlaceUnitsInCamp))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RandomEncounterUnitSelector_PlaceUnitsInCamp_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!ReplacePlaceUnitsInCampSharedYRoll(matcher, target)
                || !ReplacePlaceUnitsInCampUnitYRoll(matcher, target)
                || !ReplacePlaceUnitsInCampUnitEndPositionRoll(matcher, target))
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Failed to apply all replacements. Target={target}", target);
                return instructions;
            }

            Main.GetLogger<RestRandomEncounterContextPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
            return matcher.Instructions();
        }

        private static bool ReplacePlaceUnitsInCampSharedYRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterPlaceUnitsInCampBaseY));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        private static bool ReplacePlaceUnitsInCampUnitYRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterPlaceUnitsInCampUnitYRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldloc_S, 5),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        private static bool ReplacePlaceUnitsInCampUnitEndPositionRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterPlaceUnitsInCampUnitEndPositionRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldloc_S, 5),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }


        public static float OnEncounterPlaceUnitsInCampBaseY(float randomMin, float randomMax)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.PlaceUnitsInCampSharedYRoll = roll;
                return roll;
            }

            return context.Encounter.PlaceUnitsInCampSharedYRoll;
        }

        public static float OnEncounterPlaceUnitsInCampUnitYRoll(float randomMin, float randomMax, int index, IList<UnitEntityData> units)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;
            var unit = units[index];
            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.PlaceUnitsInCampUnitYRolls.TryAdd(unit.UniqueId, roll);
                return roll;
            }

            context.Encounter.PlaceUnitsInCampUnitYRolls.TryGetValue(unit.UniqueId, out var remoteRoll);
            return remoteRoll;
        }

        public static float OnEncounterPlaceUnitsInCampUnitEndPositionRoll(float randomMin, float randomMax, int index, IList<UnitEntityData> units)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;
            var unit = units[index];
            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.PlaceUnitsInCampUnitEndPositionRolls.TryAdd(unit.UniqueId, roll);
                return roll;
            }

            context.Encounter.PlaceUnitsInCampUnitEndPositionRolls.TryGetValue(unit.UniqueId, out var remoteRoll);
            return remoteRoll;
        }

        [HarmonyPatch(typeof(RandomEncounterUnitSelector), nameof(RandomEncounterUnitSelector.PlaceUnitsOutsideOfCamp))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RandomEncounterUnitSelector_PlaceUnitsOutsideOfCamp_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            if (!ReplacePlaceUnitsOutsideOfCampSharedYRoll(matcher, target)
                || !ReplacePlaceUnitsOutsideOfCampUnitYRoll(matcher, target)
                || !ReplacePlaceUnitsOutsideOfCampUnitEndPositionRoll(matcher, target))
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Failed to apply all replacements. Target={target}", target);
                return instructions;
            }

            Main.GetLogger<RestRandomEncounterContextPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
            return matcher.Instructions();
        }

        private static bool ReplacePlaceUnitsOutsideOfCampSharedYRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterPlaceUnitsOutsideOfCampBaseY));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        private static bool ReplacePlaceUnitsOutsideOfCampUnitYRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterPlaceUnitsOutsideOfCampUnitYRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldloc_S, 4),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }

        private static bool ReplacePlaceUnitsOutsideOfCampUnitEndPositionRoll(CodeMatcher matcher, string target)
        {
            var replaceWith = AccessTools.Method(typeof(RestRandomEncounterContextPatches), nameof(OnEncounterPlaceUnitsOutsideOfCampUnitEndPositionRoll));
            var lookFor = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), [typeof(float), typeof(float)]);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<RestRandomEncounterContextPatches>().LogError("Invalid transpiler position. Target={target}, Pos={pos}", target, match.Pos);
                return false;
            }

            match.RemoveInstruction();
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldloc_S, 4),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            return true;
        }


        public static float OnEncounterPlaceUnitsOutsideOfCampBaseY(float randomMin, float randomMax)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;

            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.PlaceUnitsOutsideOfCampSharedYRoll = roll;
                return roll;
            }

            return context.Encounter.PlaceUnitsOutsideOfCampSharedYRoll;
        }

        public static float OnEncounterPlaceUnitsOutsideOfCampUnitYRoll(float randomMin, float randomMax, int index, IList<UnitEntityData> units)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;
            var unit = units[index];
            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.PlaceUnitsOutsideOfCampUnitYRolls.TryAdd(unit.UniqueId, roll);
                return roll;
            }

            context.Encounter.PlaceUnitsOutsideOfCampUnitYRolls.TryGetValue(unit.UniqueId, out var remoteRoll);
            return remoteRoll;
        }

        public static float OnEncounterPlaceUnitsOutsideOfCampUnitEndPositionRoll(float randomMin, float randomMax, int index, IList<UnitEntityData> units)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return UnityEngine.Random.Range(randomMin, randomMax);
            }

            var context = Main.Multiplayer.RemoteContext.RandomEncounter;
            var unit = units[index];
            if (context.IsRecording)
            {
                var roll = UnityEngine.Random.Range(randomMin, randomMax);
                context.Encounter.PlaceUnitsOutsideOfCampUnitEndPositionRolls.TryAdd(unit.UniqueId, roll);
                return roll;
            }

            context.Encounter.PlaceUnitsOutsideOfCampUnitEndPositionRolls.TryGetValue(unit.UniqueId, out var remoteRoll);
            return remoteRoll;
        }
    }
}
