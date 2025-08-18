using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1047)]
    public class ClientCharacterLevelingRequested
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }
    }
}
