using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Assets.Code.UI._ConsoleUI.Overtips;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.View.MapObjects;
using WOTRMultiplayer.MP.Entities.MapObjects;

namespace WOTRMultiplayer.HarmonyPatches.MapObjects
{
    [HarmonyPatch]
    public class OvertipsPatches
    {
        /// <summary>
        /// don't be confused by Kingmaker.UI._ConsoleUI.Overtips namespace, it's still being used on PC
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="interactionPart"></param>
        [HarmonyPatch(typeof(ObjectInteractionOvertipView), nameof(ObjectInteractionOvertipView.OnClick))]
        [HarmonyPrefix]
        public static void ObjectInteractionOvertipView_OnClick_Prefix(ObjectInteractionOvertipView __instance)
        {
            var selectedUnits = Game.Instance.SelectionCharacter.SelectedUnits.Select(x => x.UniqueId).ToList();
            var networkOvertip = new NetworkOvertip
            {
                MapObjectId = __instance.MapObjectView.UniqueId,
                Units = [.. Game.Instance.SelectionCharacter.SelectedUnits.Select(x => x.UniqueId)]
            };

            Main.Multiplayer.OnInteractWithMapObjectOvertip(networkOvertip);
        }

        [HarmonyPatch(typeof(SelectionCharacterController), nameof(SelectionCharacterController.SelectedUnits), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool SelectionCharacterController_SelectedUnits_Prefix(SelectionCharacterController __instance, ref List<UnitEntityData> __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var units = Main.Multiplayer.ExecutionContext?.SelectedUnits;
            if (units == null)
            {
                return true;
            }

            __result = units;
            return false;
        }

        //[HarmonyPatch(typeof(EntityOvertipVM), nameof(EntityOvertipVM.Interact))]
        //[HarmonyTranspiler]
        //public static IEnumerable<CodeInstruction> EntityOvertipVM_Interact_Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    var attr = MethodBase.GetCurrentMethod().GetCustomAttribute<HarmonyPatch>();
        //    var target = $"{attr.info.declaringType.Name}.{attr.info.methodName}";
        //    var matcher = new CodeMatcher(instructions);
        //    var replaceWith = AccessTools.Method(typeof(OvertipsPatches), nameof(OvertipsPatches.GetSelectedUnitForInteraction));
        //    var lookFor = AccessTools.PropertyGetter(typeof(Game), nameof(Game.Instance));
        //    var match = matcher.SearchForward(x => x.Calls(lookFor));
        //    if (match == null)
        //    {
        //        Main.GetLogger<OvertipsPatches>().LogError("Transpiler has not been applied. Target={target}", target);
        //        matcher.Instructions();
        //    }

        //    match.RemoveInstructions(3);
        //    var newInstructions = new List<CodeInstruction>()
        //    {
        //        new(OpCodes.Call, replaceWith),
        //    };

        //    //match.Insert(newInstructions);
        //    Main.GetLogger<OvertipsPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
        //    Main.GetLogger<OvertipsPatches>().LogInformation(string.Join(Environment.NewLine, matcher.Instructions()));

        //    return matcher.Instructions();
        //}

        public static List<UnitEntityData> GetSelectedUnitForInteraction(InteractionPart interactionPart)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return Game.Instance.SelectionCharacter.SelectedUnits;
            }

            var units = Main.Multiplayer.ExecutionContext?.SelectedUnits;
            if (units == null)
            {
                return Game.Instance.SelectionCharacter.SelectedUnits;
            }

            return units;
        }
    }
}
