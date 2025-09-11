using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Kingmaker.UI.Selection;

namespace WOTRMultiplayer.HarmonyPatches.SelectionController
{
    [HarmonyPatch]
    public class SelectionManagerPCPatches
    {
        [HarmonyPatch(typeof(SelectionManagerPC), nameof(SelectionManagerPC.SelectUnit))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SelectionManagerPC_SelectUnit_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);

            CommonTranspilerReplacements.ReplaceIsDirectlyControllableWithLocalPlayerCheck(matcher, target, true);

            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(SelectionManagerPC), nameof(SelectionManagerPC.MultiSelect))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SelectionManagerPC_MultiSelect_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);

            CommonTranspilerReplacements.ReplaceIsDirectlyControllableWithLocalPlayerCheck(matcher, target, true);

            return matcher.Instructions();
        }
    }
}
