using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Commands.Base;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.Abstractions.PubSub;

namespace WOTRMultiplayer.PubSub
{
    public class GlobalMultiplayerUnitCommandSubscriber : GlobalMultiplayerSubscriberBase,
        IGlobalMultiplayerUnitCommandSubscriber,
        IGlobalSubscriber,
        ISubscriber,
        IUnitCommandActHandler,
        IUnitCommandEndHandler,
        IUnitRunCommandHandler
    {
        public GlobalMultiplayerUnitCommandSubscriber(
            ILogger<GlobalMultiplayerUnitCommandSubscriber> logger,
            IMultiplayerHost multiplayerHost,
            IMultiplayerClient multiplayerClient)
            : base(logger, multiplayerHost, multiplayerClient)
        {
        }

        public void HandleUnitCommandDidAct(UnitCommand command)
        {
            // this handler is not reliable for movement (UnitMoveTo), e.g. doesn't fire when you click to move far away or move(not attack) to enemy in combat
            // I assume this kind of movement is being interrupted therefor it's not treated as 'acted'
        }

        public void HandleUnitCommandDidEnd(UnitCommand command)
        {
            //if (!Host.IsActive)
            //{
            //    return;
            //}

            //var networkCommand = CreateCommand(command);
            //if (networkCommand == null)
            //{
            //    return;
            //}

            //Host.UnitCommandDidEnd(networkCommand);
        }

        public void HandleUnitRunCommand(UnitCommand cmd)
        {
        }
    }
}
