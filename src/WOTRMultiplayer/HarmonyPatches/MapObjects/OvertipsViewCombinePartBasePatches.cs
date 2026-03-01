using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI._ConsoleUI.Overtips;
using Kingmaker.View.MapObjects;
using Microsoft.Extensions.Logging;
using Owlcat.Runtime.UI.Controls.Other;
using UniRx;
using WOTRMultiplayer.Entities.MapObjects;

namespace WOTRMultiplayer.HarmonyPatches.MapObjects
{
    [HarmonyPatch]
    public class OvertipsViewCombinePartBasePatches
    {
        [HarmonyPatch(typeof(OvertipsViewCombinePartBase<OvertipsViewCombinePartEntity>), nameof(OvertipsViewCombinePartBase<OvertipsViewCombinePartEntity>.Show))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OvertipsViewCombinePartBase_BindViewImplementation_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.Field(typeof(InteractionCombineEntry), nameof(InteractionCombineEntry.Sprites));
            var extraCall = AccessTools.Method(typeof(OvertipsViewCombinePartBasePatches), nameof(OvertipsViewCombinePartBasePatches.OnShowCombinePartUI));
            var matcher = new CodeMatcher(instructions);

            var match = matcher.SearchForward(x => x.LoadsField(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<OvertipsViewCombinePartBasePatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldloc_2),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldloc_S, 5),
                new(OpCodes.Call, extraCall)
            };
            match = match.Insert(newInstructions);

            Main.GetLogger<OvertipsViewCombinePartBasePatches>().LogDebug("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        private static void OnShowCombinePartUI(OvertipsViewCombinePartBase<OvertipsViewCombinePartEntity> overtipsViewCombinePartBase, int partIndex, UnitEntityData interactedUnit, OvertipsViewCombinePartEntity entity)
        {
            // index 0 is always insert 'item' (as of now)
            // will see if this needs more specific treatment
            if (!Main.Multiplayer.IsActive || partIndex == 0)
            {
                return;
            }

            overtipsViewCombinePartBase.m_Disposables.Add(entity.Button.OnLeftClickAsObservable().Subscribe(_ => OnMapObjectCombinePartInteraction(overtipsViewCombinePartBase.ViewModel.MapObject, interactedUnit, partIndex)));
        }

        private static void OnMapObjectCombinePartInteraction(MapObjectEntityData mapObjectEntityData, UnitEntityData interactedUnit, int partIndex)
        {
            var mapObject = Main.Mapper.Map<NetworkMapObject>(mapObjectEntityData);
            var interactedUnitId = interactedUnit.UniqueId;
            Main.Multiplayer.OnMapObjectCombinePartInteraction(mapObject, interactedUnitId, partIndex);
        }
    }
}
