using System.Linq;
using System.Numerics;
using Kingmaker;
using Kingmaker.GameModes;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.View.MapObjects;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.Unity;

namespace WOTRMultiplayer.GameInteraction
{
    public class GameInteractionService : IGameInteractionService
    {
        private readonly ILogger<GameInteractionService> _logger;
        private readonly IMainThreadAccessor _mainThreadAccessor;

        public GameInteractionService(
            ILogger<GameInteractionService> logger,
            IMainThreadAccessor mainThreadAccessor)
        {
            _logger = logger;
            _mainThreadAccessor = mainThreadAccessor;
        }

        public bool IsPaused => Game.Instance.IsPaused;

        public void LeaveArea(string areaExitId)
        {
            _logger.LogInformation("Leaving area via nearest area transition");
            _mainThreadAccessor.Enqueue(() =>
            {
                var position = Game.Instance.Player.Position;
                var allTransitions = Game.Instance.State.MapObjects.All.Select(o => o.View.GetComponent<AreaTransition>()).Where(t => t != null).ToList();
                var transition = allTransitions.FirstOrDefault(x => string.Equals(x.GetComponent<MapObjectView>().UniqueId, areaExitId));
                var areaTransition = transition.GetComponent<MapObjectView>()?.Data.Get<AreaTransitionPart>();
                if (areaTransition == null)
                {
                    _logger.LogError("Unable to find requested area transition. Position={position}", position);
                    return;
                }

                Game.Instance.LoadArea(areaTransition.AreaEnterPoint, areaTransition.Settings.AutoSaveMode, null);
            });

        }

        public void MoveCharacter(string characterName, Vector3 destination, float delay, float orientation)
        {
            var character = Game.Instance.Player.PartyAndPets.FirstOrDefault(f => string.Equals(f.CharacterName, characterName));
            if (character == null)
            {
                _logger.LogError("Can't find character. Name={characterName}", characterName);
                return;
            }

            var unityDestination = new UnityEngine.Vector3(destination.X, destination.Y, destination.Z);
            var command = new UnitMoveTo(unityDestination, 0.3f)
            {
                MovementDelay = delay,
                Orientation = orientation,
                CreatedByPlayer = true
            };
            character.Commands.Run(command);
        }

        public void Pause(bool isPaused)
        {
            _logger.LogInformation("Pause game. Value={isPaused}", isPaused);
            if (isPaused)
            {
                Game.Instance.StartMode(GameModeType.Pause);
                return;
            }

            Game.Instance.StopMode(GameModeType.Pause);
        }
    }
}
