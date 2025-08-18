using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1050)]
    public class NotifyLevelingClassArchetypeSelected
    {
        [ProtoMember(1)]
        public string ArchetypeId { get; set; }
    }
}
