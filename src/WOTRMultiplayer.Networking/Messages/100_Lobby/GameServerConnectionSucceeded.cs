using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(100)]
    public class GameServerConnectionSucceeded
    {
        [ProtoMember(1)]
        public long ClientPlayerId { get; set; }

        [ProtoMember(2)]
        public NetworkGameSettings GameSettings { get; set; }
    }
}
