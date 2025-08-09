using System.Linq;
using Kingmaker;
using Kingmaker.Controllers.Rest.State;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.Abstractions.Pubsub;
using WOTRMultiplayer.MP.Entities.Rest;

namespace WOTRMultiplayer.PubSub
{
    public class MultiplayerCampingStateSubscriber : MultiplayerSubscriberBase,
        IMultiplayerGlobalSubscriber,
        IRestCraftHandler,
        IRestIterationsHandler,
        IRestScrollScribingSkillHandler,
        IRestRoleHandler,
        IRestCampUIHandler
    {
        private readonly IGameInteractionService _gameInteractionService;

        public MultiplayerCampingStateSubscriber(
            ILogger<MultiplayerCampingStateSubscriber> logger,
            IGameInteractionService gameInteractionService,
            IMultiplayerActorAccessor multiplayerActorAccessor)
            : base(logger, multiplayerActorAccessor)
        {
            _gameInteractionService = gameInteractionService;
        }

        public void HandleAlchemyChanged(bool wasSet)
        {
            OnCampingStateChanged();
        }

        public void HandleBrothChanged(bool wasSet)
        {
            OnCampingStateChanged();
        }

        public void HandleScrollChanged(bool wasSet)
        {
            OnCampingStateChanged();
        }

        public void HandleIterationsCountChanged()
        {
            OnCampingStateChanged();
        }

        public void HandleAutotuneIterationsStatusChanged()
        {
            OnCampingStateChanged();
        }

        public void HandleScrollScribingSkillChanged(bool isScrollScribingSkillArcane)
        {
            OnCampingStateChanged();
        }

        public void HandleIterationsCountCalculated(int restIterationsCount)
        {
        }

        private void OnCampingStateChanged()
        {
            try
            {
                if (ActorAccessor.Current == null)
                {
                    return;
                }

                if (!ActorAccessor.Host.IsActive)
                {
                    return;
                }

                var state = GetCampingState();

                ActorAccessor.Host.OnCampingStateChanged(state);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Unable to handle camping state change");
                throw;
            }
        }

        private NetworkCampingState GetCampingState()
        {
            var state = new NetworkCampingState
            {
                PotionBlueprintRecipeId = _gameInteractionService.CampingPotionBlueprintRecipeId,
                CookingBlueprintRecipeId = _gameInteractionService.CampingCookingBlueprintRecipeId,
                ScrollBlueprintRecipeId = _gameInteractionService.CampingScrollBlueprintRecipeId,
                AutotuneIterationsStatus = _gameInteractionService.CampingAutotuneIterationsStatus,
                IterationsCount = _gameInteractionService.CampingIterationsCount,
            };

            return state;
        }

        public void HandleUnitSetToRole(UnitReference unit, CampingRoleType type, bool isPrimary)
        {
            if (ActorAccessor.Current == null)
            {
                return;
            }


            OnCampingUnitsRoleChanged(Game.Instance.Player.Camping);
        }

        public void HandleUnitRemoveFromRole(bool changing, CampingRoleType type, bool isPrimary)
        {
            if (ActorAccessor.Current == null)
            {
                return;
            }

            OnCampingUnitsRoleChanged(Game.Instance.Player.Camping);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0031:Use null propagation", Justification = "UnitReference is a struct, but == operator is overriden within UnitReference")]
        private static void OnCampingUnitsRoleChanged(CampingState state)
        {
            var roles = state.CurrentCampingRoles.Select(x => new NetworkCampingRole
            {
                RoleType = x.Key,
                PrimaryUnitId = x.Value.m_PrimaryUnit == null ? null : x.Value.m_PrimaryUnit.UniqueId,
                SecondaryUnitId = x.Value.m_SecondaryUnit == null ? null : x.Value.m_SecondaryUnit.UniqueId,
            }).ToList();

            Main.Multiplayer.OnCampingUnitsRoleChanged(roles);
        }

        public void HandleSkipPhase()
        {
        }

        public void HandleOpenRestCamp()
        {
        }

        public void HandleVisualCampPhaseFinished()
        {
        }

        public void HandleCloseRestCamp()
        {
        }

        public void HandleShowResults()
        {
            if (ActorAccessor.Current == null)
            {
                return;
            }

            ActorAccessor.Current.OnShowRestView(Kingmaker.Controllers.Rest.RestPhase.ShowingResults);
        }
    }
}
