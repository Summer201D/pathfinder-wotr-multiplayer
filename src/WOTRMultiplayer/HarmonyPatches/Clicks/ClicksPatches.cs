using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Controllers.Clicks.Handlers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.TurnBasedMode;
using Kingmaker.View;
using Microsoft.Extensions.Logging;
using UnityEngine;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.HarmonyPatches.Clicks
{
    [HarmonyPatch]
    public class ClicksPatches
    {
        [HarmonyPatch(typeof(ClickGroundHandler), nameof(ClickGroundHandler.RunCommand))]
        [HarmonyPrefix]
        public static void ClickGroundHandler_RunCommand_Prefix(UnitEntityData unit, ClickGroundHandler.CommandSettings settings)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.MoveCharacter(unit, settings);
        }

        [HarmonyPatch(typeof(ClickUnitHandler), nameof(ClickUnitHandler.OnClick))]
        [HarmonyPostfix]
        public static void ClickUnitHandler_OnClick_Postfix(ClickUnitHandler __instance, bool __result, GameObject gameObject, Vector3 worldPosition, int button, bool simulate, bool muteEvents, bool IsTMBClick)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            if (simulate || !__result)
            {
                return;
            }

            Main.GetLogger<ClicksPatches>().LogInformation(":aaa");
            var selectedUnits = Game.Instance.SelectionCharacter.SelectedUnits.Select(x => x.UniqueId)?.ToList();
            var targetUnitId = gameObject.GetComponent<UnitEntityView>()?.UniqueId;
            var path = PathVisualizer.Instance.CurrentPathForUnit(Game.Instance.SelectionCharacter.SelectedUnits.FirstOrDefault()?.View);
            var click = new NetworkClick
            {
                SelectedUnits = selectedUnits,
                TargetUnitId = targetUnitId,
                Button = button,
                WorldPosition = new NetworkVector3(worldPosition.x, worldPosition.y, worldPosition.z),
                MuteEvents = muteEvents,
                VectorPath = [.. path?.vectorPath.Select(v => new NetworkVector3 { X = v.x, Y = v.y, Z = v.z }) ?? []]
            };

            Main.Multiplayer.OnClickUnit(click);
        }
    }
}
