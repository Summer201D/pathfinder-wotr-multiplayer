using System;
using AutoMapper;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.UI;
using Kingmaker.UI.Kingdom;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions;
using WOTRMultiplayer.Abstractions.PubSub;
using WOTRMultiplayer.Entities.GlobalMap.Kingdom;

namespace WOTRMultiplayer.Services.PubSub
{
    public class MultiplayerKingdomSubscriber : MultiplayerSubscriberBase,
        IMultiplayerGlobalSubscriber,
        IKingdomNavigationHandler,
        IEventSceneHandler
    {
        public MultiplayerKingdomSubscriber(
            ILogger<MultiplayerKingdomSubscriber> logger,
            IMultiplayerActorAccessor multiplayerActorAccessor,
            IMapper mapper)
            : base(logger, multiplayerActorAccessor, mapper)
        {
        }

        public void OnEventSelected(KingdomEventUIView kingdomEventUIView, KingdomEventHandCartController cart)
        {
            try
            {
                if (ActorAccessor.Current == null || ActorAccessor.Client.IsActive)
                {
                    return;
                }

                var kingdomEvent = Mapper.Map<NetworkKingdomEvent>(kingdomEventUIView);
                ActorAccessor.Host.OnKingdomEventSelected(kingdomEvent);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to handle kingdom event selection");
                throw;
            }
        }

        public void OnNavigationChanged(KingdomNavigationType type)
        {
            try
            {
                if (ActorAccessor.Current == null || ActorAccessor.Client.IsActive)
                {
                    return;
                }

                ActorAccessor.Host.OnKingdomNavigationChanged(type);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to handle kingdom navigation event");
                throw;
            }
        }
    }
}
