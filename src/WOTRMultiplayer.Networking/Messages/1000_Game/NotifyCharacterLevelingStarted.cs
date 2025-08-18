using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1048)]
    public class NotifyCharacterLevelingStarted
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }
    }
}
