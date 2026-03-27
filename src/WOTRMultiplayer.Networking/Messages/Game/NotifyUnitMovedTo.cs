using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyUnitMovedTo)]
    public class NotifyUnitMovedTo : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkUnitMoveTo Movement { get; set; }
    }
}
