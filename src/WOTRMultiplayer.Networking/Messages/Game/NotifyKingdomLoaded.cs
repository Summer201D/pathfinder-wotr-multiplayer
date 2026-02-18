using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyKingdomLoaded)]
    public class NotifyKingdomLoaded
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
