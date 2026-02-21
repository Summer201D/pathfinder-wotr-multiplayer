using AutoMapper;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions;

namespace WOTRMultiplayer.Services.PubSub
{
    public abstract class MultiplayerSubscriberBase
    {
        protected ILogger Logger { get; private set; }

        protected IMultiplayerActorAccessor ActorAccessor { get; private set; }

        protected IMapper Mapper { get; private set; }

        public MultiplayerSubscriberBase(
            ILogger logger,
            IMultiplayerActorAccessor multiplayerActorAccessor,
            IMapper mapper)
        {
            Logger = logger;
            ActorAccessor = multiplayerActorAccessor;
            Mapper = mapper;
        }
    }
}
