using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Controllers.Rest.State;
using Kingmaker.UI.MVVM._VM.GlobalMap;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapVMPatches
    {
        [HarmonyPatch(typeof(GlobalMapVM), nameof(GlobalMapVM.HandleRestShowRequest))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GlobalMapVM_HandleRestShowRequest_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var extraCall = AccessTools.Method(typeof(GlobalMapVMPatches), nameof(GlobalMapVMPatches.OnGlobalMapRestOpened));
            var lookFor = AccessTools.Method(typeof(CampingState), nameof(CampingState.Init));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<GlobalMapVMPatches>().LogError("Invalid transpiler position. Target={Target}, Position={Position}", target, match.Pos);
                return instructions;
            }

            match = match.Advance(-4);
            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, extraCall),
            };
            match.Insert(newInstructions);
            Main.GetLogger<GlobalMapVMPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        public static void OnGlobalMapRestOpened()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapRestOpened();
        }
    }
}
