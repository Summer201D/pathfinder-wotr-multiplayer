using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyKingdomEventDropped)]
    public class NotifyKingdomEventDropped
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomEvent Event { get; set; }
    }
}
