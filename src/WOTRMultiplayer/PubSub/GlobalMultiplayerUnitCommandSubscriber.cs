using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.Abstractions.PubSub;
using WOTRMultiplayer.MP.Entities;

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

        private NetworkUnitCommand CreateCommand(UnitCommand command)
        {
            var type = GetTypeFromCommand(command);
            if (type == NetworkUnitCommandType.Ignored)
            {
                Logger.LogInformation("Command has been ignored. CommandType={commandType}", command.GetType().Name);
                return null;
            }

            var networkUnitCommand = new NetworkUnitCommand
            {
                TargetId = command.Target?.Unit?.UniqueId,
                CommandType = type,
                UnitId = command.Executor?.UniqueId
            };

            if (command is UnitMoveTo moveTo)
            {
                networkUnitCommand.Destination = new System.Numerics.Vector3(moveTo.Target.x, moveTo.Target.y, moveTo.Target.z);
                networkUnitCommand.Orientation = moveTo.Orientation;
            }
            else if (command is UnitAttack)
            {
            }

            return networkUnitCommand;
        }

        private NetworkUnitCommandType GetTypeFromCommand(UnitCommand command)
        {
            return command switch
            {
                UnitMoveTo => NetworkUnitCommandType.Move,
                UnitAttack => NetworkUnitCommandType.Attack,

                UnitAttackOfOpportunity => NetworkUnitCommandType.Ignored,
                _ => NetworkUnitCommandType.Unknown
            };
        }
    }
}
