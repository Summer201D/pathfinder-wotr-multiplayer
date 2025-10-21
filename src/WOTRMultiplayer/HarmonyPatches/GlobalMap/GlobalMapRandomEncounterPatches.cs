using System.Linq;
using HarmonyLib;
using Kingmaker.Assets.Controllers.GlobalMap;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Globalmap;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Globalmap.State;
using Kingmaker.Globalmap.View;
using Kingmaker.UI;
using Kingmaker.UI.Common;
using Kingmaker.UI.GlobalMap;
using Kingmaker.UI.MVVM._PCView.GlobalMap.Message;
using Kingmaker.UI.MVVM._VM.GlobalMap.Message;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.MP.Entities.GlobalMap;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapRandomEncounterPatches
    {
        [HarmonyPatch(typeof(RandomEncountersController), nameof(RandomEncountersController.RollTravelEncounter))]
        [HarmonyPrefix]
        public static bool RandomEncountersController_RollTravelEncounter_Prefix(ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.OnGlobalMapBeforeRollTravelEncounter();
            if (!canContinue)
            {
                __result = false;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(GlobalMapRandomEncounterController), nameof(GlobalMapRandomEncounterController.OnRandomEncounterStarted))]
        [HarmonyPrefix]
        public static void GlobalMapRandomEncounterController_OnRandomEncounterStarted_Prefix()
        {
            Main.GetLogger<GlobalMapRandomEncounterPatches>().LogWarning("On Random Encounter");
        }

        [HarmonyPatch(typeof(GlobalMapPlayerState), nameof(GlobalMapPlayerState.StartTravel))]
        [HarmonyPrefix]
        public static void GlobalMapPlayerState_StartTravel_Prefix(GlobalMapTravelData travelData)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // not sure if location is always available during act2+ travels due to navigation arrows
            var destination = GetNetworkGlobalMapLocation(travelData.To.Location);

            Main.Multiplayer.OnGlobalMapStartTravel(destination);
        }

        [HarmonyPatch(typeof(GlobalMapPlayerState), nameof(GlobalMapPlayerState.FinishTravel))]
        [HarmonyPrefix]
        public static void GlobalMapPlayerState_FinishTravel_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.GetLogger<GlobalMapRandomEncounterPatches>().LogWarning("GlobalMapPlayerState_FinishTravel_Prefix");
        }

        [HarmonyPatch(typeof(GlobalMapEnterMessagePCView), nameof(GlobalMapEnterMessagePCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void GlobalMapEnterMessagePCView_BindViewImplementation_Postfix(GlobalMapEnterMessagePCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapMessageBoxShown();
        }

        [HarmonyPatch(typeof(GlobalMapEnterMessageVM), nameof(GlobalMapEnterMessageVM.Close))]
        [HarmonyPrefix]
        public static void GlobalMapEnterMessageVM_Close_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapMessageBoxClosed();
        }

        [HarmonyPatch(typeof(GlobalMapEnterMessageVM), nameof(GlobalMapEnterMessageVM.CanLocationSelect))]
        [HarmonyPostfix]
        public static void GlobalMapEnterMessageVM_CanLocationSelect_Prefix(GlobalMapPointView locationView, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var location = GetNetworkGlobalMapLocation(locationView.Blueprint);
            var canSelectLocation = Main.Multiplayer.OnGlobalMapSelectLocation(location);
            __result = __result && canSelectLocation;
        }

        [HarmonyPatch(typeof(GlobalMapUI), nameof(GlobalMapUI.OnContinue))]
        [HarmonyPrefix]
        public static void GlobalMapUI_OnContinue_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var globalMapState = GetGlobalMapState();
            Main.Multiplayer.OnGlobalMapContinueTravel(globalMapState);
        }

        [HarmonyPatch(typeof(GlobalMapUI), nameof(GlobalMapUI.OnStop))]
        [HarmonyPrefix]
        public static void GlobalMapUI_OnStop_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var globalMapState = GetGlobalMapState();
            Main.Multiplayer.OnGlobalMapStopTravel(globalMapState);
        }

        [HarmonyPatch(typeof(GlobalMapMovementUtility), nameof(GlobalMapMovementUtility.ShowCollectIngredientMessage))]
        [HarmonyPrefix]
        public static bool GlobalMapMovementUtility_ShowCollectIngredientMessage_Prefix(IGlobalMapTraveler traveler, GlobalMapPointState pointState)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // looks super weird to use different approach to show message box confirmation for ingredient collection, but it is what it is
            // slightly modified copy paste of the original method since creating transpiler for compiler generated classes is way worse
            var craftRoot = BlueprintRoot.Instance.CraftRoot.CollectRoot;
            if (pointState.IngredientWasCollected)
            {
                // this one doesn't make sense to sync, so we just ignore it
                UIUtility.ShowMessageBox(craftRoot.AlreadyCollected, MessageModalBase.ModalType.Message, null);
                return false;
            }

            Main.Multiplayer.OnGlobalMapIngredientCollectionShown();
            UIUtility.ShowMessageBox(craftRoot.PointResources, MessageModalBase.ModalType.Dialog, delegate (MessageModalBase.ButtonType result)
            {
                if (result != MessageModalBase.ButtonType.Yes)
                {
                    Main.Multiplayer.OnGlobalMapIngredientCollectionClosed();
                    return;
                }

                var collected = craftRoot.CollectIngredient(traveler.Location);
                var warningMessage = collected.Count > 0 ? craftRoot.SuccessCollect : craftRoot.FailCollected;
                UIUtility.SendWarning(warningMessage, addLog: false);
                Kingmaker.PubSubSystem.EventBus.RaiseEvent<Kingmaker.PubSubSystem.ILogMessageUIHandler>(x => x.HandleLogMessage((collected.Count > 0) ? $"{craftRoot.SuccessCollect}:\n{BlueprintGlobalMapPoint.IngredientToString(collected)}" : ((string)craftRoot.FailCollected)));
                pointState.IngredientWasCollected = true;
                pointState.SetVisited();

                var location = GetNetworkGlobalMapLocation(traveler.Location);
                Main.Multiplayer.OnGlobalMapIngredientCollectionAccepted(location);
            }, null, 0, UIStrings.Instance.Tooltips.Collect, null, [.. traveler.Location.Ingredients.Select(i => i.Ingredient.Get())]);

            return false;
        }

        private static NetworkGlobalMapState GetGlobalMapState()
        {
            var state = new NetworkGlobalMapState
            {
                Player = new NetworkGlobalMapTraveler
                {
                    Position = GetGlobalMapPosition(GlobalMapView.Instance.State.Player?.Position),
                },
            };
            return state;
        }

        private static NetworkGlobalMapPosition GetGlobalMapPosition(GlobalMapPosition globalMapPosition)
        {
            if (globalMapPosition == null)
            {
                return null;
            }

            var position = new NetworkGlobalMapPosition
            {
                Edge = globalMapPosition.EdgePosition
            };
            return position;
        }

        private static NetworkGlobalMapLocation GetNetworkGlobalMapLocation(BlueprintGlobalMapPoint globalMapPoint)
        {
            var location = new NetworkGlobalMapLocation
            {
                Id = globalMapPoint.AssetGuid.ToString(),
                Name = globalMapPoint.name,
            };

            return location;
        }
    }
}
