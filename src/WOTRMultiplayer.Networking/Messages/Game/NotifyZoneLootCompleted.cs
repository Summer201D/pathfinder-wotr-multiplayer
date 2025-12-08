using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyZoneLootCompleted)]
    public class NotifyZoneLootCompleted
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
