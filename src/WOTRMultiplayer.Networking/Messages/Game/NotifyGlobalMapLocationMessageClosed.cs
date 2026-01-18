using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapLocationMessageClosed)]
    public class NotifyGlobalMapLocationMessageClosed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
