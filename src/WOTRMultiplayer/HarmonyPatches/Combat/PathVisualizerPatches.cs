using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.TurnBasedMode;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class PathVisualizerPatches
    {
        [HarmonyPatch(typeof(PathVisualizer), nameof(PathVisualizer.Update))]
        [HarmonyPrefix]
        public static bool PathVisualizer_Update_Prefix(PathVisualizer __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var rider = Game.Instance.TurnBasedCombatController.CurrentTurn?.Rider;
            if (Main.Multiplayer.IsControlledByLocalPlayer(rider?.UniqueId))
            {
                return true;
            }

            // casting sticky touch ability (e.g. Cure Wounds) actually creates x2 ability usages.
            // Although we set ForcedPath for the first command, it's not propagated directly to the second one.
            // Second command relies on PathVisualizer.Instance.m_currentPath value to move caster to target in combat
            // so it must not be cleared while we are casting original ability. And that's the reason why we can't enable path visualizer for players who don't own the current turn, as it will corrupt path on any update (e.g. mouse movement)
            if (rider?.Commands.UnitUseAbility == null && rider?.Commands.Attack == null)
            {
                __instance.Clear();
            }

            return false;
        }

        [HarmonyPatch(typeof(PathVisualizer), nameof(PathVisualizer.SetGradientsToRenderer))]
        [HarmonyPrefix]
        public static bool PathVisualizer_SetGradientsToRenderer_Prefix(PathVisualizer __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // it fails with NRE anyway, just less error logs
            return __instance.m_VisualPath.Any();
        }
    }
}
