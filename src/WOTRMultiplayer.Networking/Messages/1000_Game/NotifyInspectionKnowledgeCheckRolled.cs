using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1040)]
    public class NotifyInspectionKnowledgeCheckRolled
    {
        [ProtoMember(1)]
        public NetworkInspectionKnowledgeCheck Check { get; set; }
    }
}
