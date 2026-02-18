using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyKingdomUnloaded)]
    public class NotifyKingdomUnloaded
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
