using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1027)]
    public class NotifyDropItem
    {
        [ProtoMember(1)]
        public NetworkDropItem Drop { get; set; }
    }
}
