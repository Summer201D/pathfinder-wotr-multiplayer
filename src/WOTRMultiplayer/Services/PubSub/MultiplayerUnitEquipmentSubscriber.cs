using System;
using AutoMapper;
using Kingmaker.ElementsSystem;
using Kingmaker.GameModes;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.PubSub;
using WOTRMultiplayer.Entities.Equipment;

namespace WOTRMultiplayer.Services.PubSub
{
    public class MultiplayerUnitEquipmentSubscriber : MultiplayerSubscriberBase,
        IMultiplayerGlobalSubscriber,
        IUnitEquipmentHandler,
        IUnitActiveEquipmentSetHandler
    {
        private readonly IGameInteractionService _gameInteractionService;

        public MultiplayerUnitEquipmentSubscriber(
            ILogger<MultiplayerUnitEquipmentSubscriber> logger,
            IGameInteractionService gameInteractionService,
            IMultiplayerActorAccessor multiplayerActorAccessor,
            IMapper mapper)
            : base(logger, multiplayerActorAccessor, mapper)
        {
            _gameInteractionService = gameInteractionService;
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            try
            {
                // ignoring when:
                // - multiplayer is not active
                // - game is loading new game
                // - previousItem exists, but has no collection = either used by player (potion) or some scripted action
                // - alchemist bombs
                if (ActorAccessor.Current == null
                        || _gameInteractionService.CurrentGameMode == GameModeType.None
                        || (previousItem != null && previousItem.Collection == null && !slot.HasItem)
                        || ContextData<FastBombs.FastBombsContext>.Current != null)
                {
                    return;
                }

                var position = _gameInteractionService.GetEquipmentSlotPosition(slot);
                if (position == null || position.Index == -1)
                {
                    return;
                }

                var swapContext = ContextData<ItemsCollection.SwapItems>.Current;
                var equipmentSwapContext = swapContext == null ? null : new NetworkEquipmentSwapContext
                {
                    From = _gameInteractionService.GetEquipmentSlotPosition(swapContext.From),
                    To = _gameInteractionService.GetEquipmentSlotPosition(swapContext.To)
                };

                var networkSlot = new NetworkEquipmentSlot
                {
                    Item = Main.Mapper.Map<NetworkItem>(slot.MaybeItem),
                    SwapContext = equipmentSwapContext,
                    OwnerId = slot.Owner.Unit.UniqueId,
                    Position = position
                };

                var equipmentContext = _gameInteractionService.RemoteContext?.Equipment;
                if (equipmentContext != null && equipmentContext.Position.Type == position.Type && equipmentContext.Position.Index == position.Index)
                {
                    return;
                }

                ActorAccessor.Current.OnEquipmentSlotChanged(networkSlot);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to handle equipment change");
            }
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            if (ActorAccessor.Current == null)
            {
                return;
            }

            var networkActiveHandEquipmentSet = new NetworkActiveHandEquipmentSet
            {
                Index = unit.Body.CurrentHandEquipmentSetIndex,
                UnitId = unit.Unit.UniqueId,
            };

            var context = _gameInteractionService.RemoteContext?.HandEquipment;
            if (context != null
                && context.Index == networkActiveHandEquipmentSet.Index
                && string.Equals(context.UnitId, networkActiveHandEquipmentSet.UnitId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            ActorAccessor.Current.OnChangeActiveHandEquipmentSet(networkActiveHandEquipmentSet);
        }
    }
}
