using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Blueprints.Area;
using Kingmaker.Controllers;
using Kingmaker.Kingdom.Settlements;
using Kingmaker.UI.Common;
using Kingmaker.UI.GlobalMap;
using Kingmaker.UI.Kingdom;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Entities.GlobalMap.Kingdom;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap.Kingdom
{
    [HarmonyPatch]
    public class KingdomControllerPatches
    {
        [HarmonyPatch(typeof(GlobalMapKingdom), nameof(GlobalMapKingdom.OnKingdomClick))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GlobalMapKingdom_OnKingdomClick_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var extraCall = AccessTools.Method(typeof(KingdomControllerPatches), nameof(KingdomControllerPatches.OnEnterKingdomArea));
            var lookFor = AccessTools.Method(typeof(KingdomController), nameof(KingdomController.EnterKingdomArea));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<KingdomControllerPatches>().LogError("Invalid transpiler position. Target={Target}", target);
                return instructions;
            }

            var newInstruction = new List<CodeInstruction>()
            {
                new(OpCodes.Call, extraCall),
            };
            match = match.Advance(1).Insert(newInstruction);
            Main.GetLogger<KingdomControllerPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(UIUtility), nameof(UIUtility.EnterKingdom))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> UIUtility_EnterKingdom_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var extraCall = AccessTools.Method(typeof(KingdomControllerPatches), nameof(KingdomControllerPatches.OnEnterKingdomArea));
            var lookFor = AccessTools.Method(typeof(KingdomController), nameof(KingdomController.EnterKingdomArea));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.SearchForward(x => x.Calls(lookFor));

            if (match.IsInvalid)
            {
                Main.GetLogger<KingdomControllerPatches>().LogError("Invalid transpiler position. Target={Target}", target);
                return instructions;
            }

            var newInstruction = new List<CodeInstruction>()
            {
                new(OpCodes.Call, extraCall),
            };
            match = match.Advance(1).Insert(newInstruction);
            Main.GetLogger<KingdomControllerPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(KingdomStaticCanvas), nameof(KingdomStaticCanvas.OnExitButton))]
        [HarmonyPrefix]
        public static bool KingdomStaticCanvas_OnExitButton_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            if (!Main.Multiplayer.CanControlGlobalMap())
            {
                return false;
            }

            Main.Multiplayer.OnExitKingdom();
            return true;
        }

        private static void OnLeaveKingdom()
        {

        }

        private static void OnEnterKingdomArea()
        {
            OnEnterKingdomArea(null, null);
        }

        private static void OnEnterKingdomArea(BlueprintAreaEnterPoint returnPoint, SettlementState settlementToFocus)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var kingdomEntryPoint = new NetworkKingdomEntryPoint
            {
                Id = returnPoint?.AssetGuid.ToString(),
                Settlement = Main.Mapper.Map<NetworkKingdomSettlement>(settlementToFocus)
            };

            Main.Multiplayer.OnEnterKingdom(kingdomEntryPoint);
        }
    }
}
