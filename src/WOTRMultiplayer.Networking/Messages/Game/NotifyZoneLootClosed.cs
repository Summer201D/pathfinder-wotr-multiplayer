using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyZoneLootClosed)]
    public class NotifyZoneLootClosed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
