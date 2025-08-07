using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(101)]
    public class ClientGameServerConnectionConfirmed
    {
        [ProtoMember(1)]
        public string PlayerName { get; set; }
    }
}
