using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1049)]
    public class NotifyLevelingClassSelected
    {
        [ProtoMember(1)]
        public string ClassId { get; set; }
    }
}
