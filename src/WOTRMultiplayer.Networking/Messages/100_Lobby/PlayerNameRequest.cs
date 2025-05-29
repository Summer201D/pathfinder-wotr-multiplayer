using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(100)]
    public class PlayerNameRequest
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
