using HarmonyLib;
using Kingmaker;
using Kingmaker.Controllers.Clicks.Handlers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.View.MapObjects;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.UI;

namespace WOTRMultiplayer.HarmonyPatches.PubSub
{
    [HarmonyPatch]
    public class PubSubPatches
    {
        //[HarmonyPatch(typeof(SubscribersList<object>), nameof(SubscribersList<object>.AddSubscriber))]
        //[HarmonyPrefix]
        //public static bool SubscribersList_AddSubscriber_Prefix(SubscribersList<object> __instance, object subscriber)
        //{
        //    var logger = Main.GetLogger<SubscriberListPatches>();

        //    logger.LogInformation("GenericType={genericType}, SubscriberType={subscriberType}", __instance.GetType().Name, subscriber?.GetType().Name);

        //    return false;
        //}

        //[HarmonyPatch(typeof(SubscriptionManager<IGlobalSubscriber>), nameof(SubscriptionManager<IGlobalSubscriber>.Subscribe))]
        //[HarmonyPrefix]
        //public static void SubscriptionManager_Subscribe_Prefix(SubscriptionManager<IGlobalSubscriber> __instance, object subscriber, ISubscriptionProxy proxy)
        //{
        //    //var logger = Main.GetLogger<PubSubPatches>();

        //    //logger.LogInformation("GenericType={genericType}, SubscriberType={subscriberType}", __instance.GetType().GenericTypeArguments?.FirstOrDefault(), subscriber?.GetType().Name);

        //    //if (subscriber is INetworkSubscriber)
        //    //{
        //    //    logger.LogInformation("Network sub");
        //    //}

        //    //return true;
        //}

        [HarmonyPatch(typeof(EventBus), nameof(EventBus.Subscribe), typeof(IUnitSubscriber), typeof(ISubscriptionProxy))]
        [HarmonyPrefix]
        public static void EventBus_Subscribe_Prefix(IUnitSubscriber subscriber, ISubscriptionProxy proxy)
        {
            //var logger = Main.GetLogger<EventBus>();
            //var unit = subscriber?.GetSubscribingUnit() ?? proxy?.GetSubscribingUnit();
            //if (unit != null)
            //{
            //    logger.LogInformation("Subscribe. SubscriberType={subscriberType} CharacterName={charactrerName}", (subscriber?.GetType() ?? proxy?.GetSubscriber()?.GetType()).Name, unit.CharacterName);
            //}
        }

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

        [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.IsDirectlyControllable), MethodType.Getter)]
        [HarmonyPostfix]
        public static void UnitEntityData_IsDirectlyControllable_Postfix(UnitEntityData __instance, ref bool __result)
        {
            if (!__result || !Main.Multiplayer.IsActive)
            {
                return;
            }

            if (__instance.IsPet && __instance.Master == null)
            {
                Main.GetLogger<PubSubPatches>().LogError("Pet has no master, but still controllable");
                return;
            }

            var characterName = __instance.IsPet ? __instance.Master.CharacterName : __instance.CharacterName;
            __result = Main.Multiplayer.CanControlCharacter(characterName);
        }

        [HarmonyPatch(typeof(Game), nameof(Game.StartMode))]
        [HarmonyPrefix]
        public static bool Game_StartMode_Prefix(Game __instance, GameModeType type)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var allowedToRun = Main.Multiplayer.StartGameMode(type);
            return allowedToRun;
        }

        [HarmonyPatch(typeof(Game), nameof(Game.StopMode))]
        [HarmonyPrefix]
        public static bool Game_StopMode_Prefix(Game __instance, GameModeType type)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var allowedToRun = Main.Multiplayer.StopGameMode(type);
            return allowedToRun;
        }

        [HarmonyPatch(typeof(AreaTransitionPart), nameof(AreaTransitionPart.CheckRestrictions))]
        [HarmonyPostfix]
        public static void AreaTransitionPart_CheckRestrictions_Postfix(AreaTransitionPart __instance, UnitEntityData user, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive || !__result)
            {
                return;
            }

            if (!Main.Multiplayer.CanLeaveArea())
            {
                Game.Instance.UI.Bark(user, UIStringConsts.GameNotifications.TryLeaveAsAClient, 10f);
                __result = false;
            }
        }
    }
}
