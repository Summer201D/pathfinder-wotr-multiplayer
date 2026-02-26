using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Kingmaker.UI;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Bindings
{
    [HarmonyPatch]
    public class KeyboardAccessPatches
    {
        [HarmonyPatch(typeof(KeyboardAccess), nameof(KeyboardAccess.Bind))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> KeyboardAccess_Bind_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.PropertyGetter(typeof(PFLog), nameof(PFLog.Default));
            var replaceWith = AccessTools.Method(typeof(KeyboardAccessPatches), nameof(KeyboardAccessPatches.OnLogMissingBinding));
            var matcher = new CodeMatcher(instructions);

            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<KeyboardAccessPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstructions(6).Advance(2).RemoveInstructions(2).Insert(newInstructions);
            Main.GetLogger<KeyboardAccessPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(KeyboardAccess), nameof(KeyboardAccess.DoUnbind))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> KeyboardAccess_DoUnbind_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.PropertyGetter(typeof(PFLog), nameof(PFLog.Default));
            var replaceWith = AccessTools.Method(typeof(KeyboardAccessPatches), nameof(KeyboardAccessPatches.OnLogMissingBinding));
            var matcher = new CodeMatcher(instructions);

            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<KeyboardAccessPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, replaceWith)
            };
            match = match.RemoveInstructions(6).Advance(2).RemoveInstructions(2).Insert(newInstructions);
            Main.GetLogger<KeyboardAccessPatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static void OnLogMissingBinding(string bindingName)
        {
            if (!Main.Multiplayer.IsActive)
            {
                PFLog.Default.Warning("Bind: no binding named {0}", [bindingName]);
                return;
            }
        }
    }
}
