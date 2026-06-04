using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLootClosed)]
    public class NotifyLootClosed : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkMapObject MapObject { get; set; }
    }
}
