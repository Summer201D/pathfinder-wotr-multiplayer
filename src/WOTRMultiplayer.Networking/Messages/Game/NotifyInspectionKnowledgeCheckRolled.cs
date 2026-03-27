using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyInspectionKnowledgeCheckRolled)]
    public class NotifyInspectionKnowledgeCheckRolled
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkInspectionKnowledgeCheck Check { get; set; }
    }
}
