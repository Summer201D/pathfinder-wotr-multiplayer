using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapMessageBoxClosed)]
    public class NotifyGlobalMapMessageBoxClosed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
