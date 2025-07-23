using AutoMapper;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.MP
{
    public abstract class MultiplayerActorBase
    {
        public const int LocalHostPlayerId = -1;

        protected NetworkGame Game { get; set; }

        protected ILogger Logger { get; private set; }

        protected IMapper Mapper { get; private set; }

        protected abstract bool IsHost { get; }

        protected MultiplayerActorBase(
            ILogger logger,
            IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }

        protected bool IsRolledByHost(bool silent)
        {
            var isNotInCombat = Game.Combat == null;
            var isCombatNotInitialized = !(Game.Combat?.IsInitialized ?? false);
            var isTurnNotInitialized = Game.Combat?.Turn == null;
            var isAI = Game.Combat?.Turn?.IsAI ?? false;
            var result = isNotInCombat // everything happens on host outside of combat
                || isCombatNotInitialized // combat initialization phase (initiative rolls)
                || isTurnNotInitialized // could happen when some new NPC joins midfight in midturns, e.g. Anevia in prologue
                || isAI; // clients are getting their AI rolls from host

            if (!silent)
            {
                Logger.LogInformation("IsRolledByHost calculation. Result={result}, IsNotInCombat={isNotInCombat}, IsCombatNotInitialized={isCombatNotInitialized}, IsTurnNotInitialized={isTurnNotInitialized}, IsAI={isAI}",
                    result, isNotInCombat, isCombatNotInitialized, isTurnNotInitialized, isAI);
            }

            return result;
        }

        protected bool IsRolledByLocalPlayer(bool silent)
        {
            var isNotAI = !(Game.Combat?.Turn?.IsAI ?? false);
            var isLocalPlayer = Game.Combat?.Turn?.IsLocalPlayer ?? false;
            var result = isNotAI  // clients are getting their AI rolls from host
                && isLocalPlayer; // other MP players are getting rolls from turn owner

            if (!silent)
            {
                Logger.LogInformation("IsRolledByLocalPlayer calculation. Result={result}, IsNotAI={isNotAI}, IsLocalPlayer={isLocalPlayer}",
                result, isNotAI, isLocalPlayer);
            }
            return result;
        }
    }
}
