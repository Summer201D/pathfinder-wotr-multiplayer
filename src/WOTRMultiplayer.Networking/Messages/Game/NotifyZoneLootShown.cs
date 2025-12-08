using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyZoneLootShown)]
    public class NotifyZoneLootShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
