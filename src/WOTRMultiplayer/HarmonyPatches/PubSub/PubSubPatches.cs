using System;
using System.Text;
using System.Threading;
using HarmonyLib;
using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Controllers.Clicks.Handlers;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.View.MapObjects;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.UI;
using static Kingmaker.AreaLogic.Capital.CapitalCompanionLogic;

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

        [HarmonyPatch(typeof(DialogOnClick), nameof(DialogOnClick.Interact))]
        [HarmonyPostfix]
        public static void DialogOnClick_Interact_Postfix(DialogOnClick __instance, UnitEntityData user, UnitEntityData target, ref UnitCommand.ResultType __result)
        {
            if (!Main.Multiplayer.IsActive || __result != UnitCommand.ResultType.Success)
            {
                return;
            }

            var dialogueId = __instance.Dialog?.name;
            var userId = user?.UniqueId;
            var targetId = target?.UniqueId;

            Main.GetLogger<PubSubPatches>().LogWarning("DialogOnClick. DialogueId={dialogueId}, UserId={userId}, TargetId={targetId}", dialogueId, userId, targetId);
        }


        [HarmonyPatch(typeof(OverrideDialogInteraction), nameof(OverrideDialogInteraction.Interact))]
        [HarmonyPostfix]
        public static void OverrideDialogInteraction_Interact_Postfix(OverrideDialogInteraction __instance, UnitEntityData user, UnitEntityData target, ref UnitCommand.ResultType __result)
        {
            if (!Main.Multiplayer.IsActive || __result != UnitCommand.ResultType.Success)
            {
                return;
            }

            var dialogueId = __instance.Dialog?.name;
            var userId = user?.UniqueId;
            var targetId = target?.UniqueId;

            Main.GetLogger<PubSubPatches>().LogWarning("OverrideDialogInteraction. DialogueId={dialogueId}, UserId={userId}, TargetId={targetId}", dialogueId, userId, targetId);
        }

        [HarmonyPatch(typeof(EtudeBracketOverrideDialog), nameof(EtudeBracketOverrideDialog.Interact))]
        [HarmonyPostfix]
        public static void EtudeBracketOverrideDialog_Interact_Postfix(EtudeBracketOverrideDialog __instance, UnitEntityData user, UnitEntityData target, ref UnitCommand.ResultType __result)
        {
            if (!Main.Multiplayer.IsActive || __result != UnitCommand.ResultType.Success)
            {
                return;
            }

            var dialogueId = __instance.Dialog?.Guid.ToString();
            var userId = user?.UniqueId;
            var targetId = target?.UniqueId;

            Main.GetLogger<PubSubPatches>().LogWarning("EtudeBracketOverrideDialog. DialogueId={dialogueId}, UserId={userId}, TargetId={targetId}", dialogueId, userId, targetId);
        }

        [HarmonyPatch(typeof(SpawnerInteractionDialog), nameof(SpawnerInteractionDialog.Interact))]
        [HarmonyPostfix]
        public static void SpawnerInteractionDialog_Interact_Postfix(SpawnerInteractionDialog __instance, UnitEntityData user, UnitEntityData target, ref UnitCommand.ResultType __result)
        {
            if (!Main.Multiplayer.IsActive || __result != UnitCommand.ResultType.Success)
            {
                return;
            }

            var dialogueId = __instance.Dialog?.name;
            var userId = user?.UniqueId;
            var targetId = target?.UniqueId;

            Main.GetLogger<PubSubPatches>().LogWarning("SpawnerInteractionDialog. DialogueId={dialogueId}, UserId={userId}, TargetId={targetId}", dialogueId, userId, targetId);
        }

        [HarmonyPatch(typeof(DialogController), nameof(DialogController.StartDialogWithUnit))]
        [HarmonyPrefix]
        public static void DialogController_Interact_Postfix(DialogController __instance, BlueprintDialog dialog, UnitEntityData unit, UnitEntityData initiator)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var dialogueId = dialog?.name;
            var userId = initiator?.UniqueId;
            var targetId = unit?.UniqueId;

            Main.GetLogger<PubSubPatches>().LogWarning("DialogController. DialogueId={dialogueId}, UserId={userId}, TargetId={targetId}", dialogueId, userId, targetId);
        }

        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.OnTrigger))]
        [HarmonyPostfix]
        public static void RuleRollDice_OnTrigger_Postfix(RuleRollDice __instance, RulebookEventContext context)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var initiator = __instance.Initiator?.UniqueId;
            var dice = __instance.DiceFormula.Dice;
            var resultOverride = __instance.ResultOverride;
            var result = __instance.Result;
            var ruleType = __instance.Reason.Rule.GetType().Name;
            var ruleName = __instance.Reason.Name;

            switch (__instance.Reason.Rule)
            {
                case RulePartyStatCheck rulePartyStatCheck:
                    var rollUniqueId = Convert.ToBase64String(Encoding.UTF8.GetBytes(initiator + dice + ruleType + ruleName + rulePartyStatCheck.DifficultyClass + rulePartyStatCheck.StatType));
                    Main.GetLogger<PubSubPatches>().LogWarning("RuleRollDice_OnTrigger_Postfix. Initiator={initiator}, Dice={dice}, ResultOverride={resultOverride}, Result={result}, RuleName={ruleName} RuleType={ruleType}, RuleUniqueId={rollUniqueId}", initiator, dice, resultOverride, result, ruleName, ruleType, rollUniqueId);
                    break;
                case RuleRollD20:
                default:
                    Main.GetLogger<PubSubPatches>().LogWarning("RuleRollDice_OnTrigger_Postfix - Skipping dice roll. Type={rollType}", __instance.Reason.Rule.GetType().Name);
                    break;
            }
        }

        [HarmonyPatch(typeof(RuleRollDice), nameof(RuleRollDice.OnTrigger))]
        [HarmonyPrefix]
        public static bool RuleRollDice_OnTrigger_Prefix(RuleRollDice __instance, RulebookEventContext context)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var initiator = __instance.Initiator?.UniqueId;
            var dice = __instance.DiceFormula.Dice;
            var resultOverride = __instance.ResultOverride;
            var result = __instance.Result;
            var ruleType = __instance.Reason.Rule.GetType().Name;
            var ruleName = __instance.Reason.Name;

            switch (__instance.Reason.Rule)
            {
                case RulePartyStatCheck rulePartyStatCheck:
                    var rollUniqueId = Convert.ToBase64String(Encoding.UTF8.GetBytes(initiator + dice + ruleType + ruleName + rulePartyStatCheck.DifficultyClass + rulePartyStatCheck.StatType));
                    if (rollUniqueId == "YTk1MGFkNzUtNjVjZC00ZGMxLTk2ZTktNDQ0ZTI5MWZlZDdlRDIwUnVsZVBhcnR5U3RhdENoZWNrMTJDaGVja0RpcGxvbWFjeQ==")
                    {
                        Thread.Sleep(200); // network latency
                        Main.GetLogger<PubSubPatches>().LogWarning("RuleRollDice_OnTrigger_Prefix - Using pregenerated values for RulePartyStatCheck");
                        __instance.m_Result = 36;
                        return false;
                    }
                    break;
                case RuleRollD20:
                default:
                    break;
            }

            return true;
        }
    }
}
