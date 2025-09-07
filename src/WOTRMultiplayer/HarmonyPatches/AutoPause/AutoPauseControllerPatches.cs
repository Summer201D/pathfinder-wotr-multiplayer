using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.View.MapObjects.Traps;
using TurnBased.Controllers;

namespace WOTRMultiplayer.HarmonyPatches.AutoPause
{
    [HarmonyPatch]
    public class AutoPauseControllerPatches
    {
        [HarmonyPatch(typeof(AutoPauseController), nameof(AutoPauseController.Pause))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AutoPauseController_Pause_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);

            CommonTranspilerReplacements.ReplaceIsDirectlyControllable(matcher, target);

            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(AutoPauseController), nameof(AutoPauseController.OnEntityNoticed))]
        [HarmonyPostfix]
        public static void AutoPauseController_OnEntityNoticed_Postfix(StaticEntityData entity, UnitEntityData character)
        {
            if (!Main.Multiplayer.IsActive || entity is not TrapObjectData || CombatController.IsInTurnBasedCombat())
            {
                return;
            }

            Main.Multiplayer.OnAutoPausedByTrapDetection();
        }
    }
}
