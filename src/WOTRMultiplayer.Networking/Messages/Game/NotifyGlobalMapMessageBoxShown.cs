using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapMessageBoxShown)]
    public class NotifyGlobalMapMessageBoxShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
