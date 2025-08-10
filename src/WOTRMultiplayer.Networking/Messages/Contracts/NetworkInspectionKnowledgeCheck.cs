using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkInspectionKnowledgeCheck
    {
        [ProtoMember(1)]
        public string TargetUnitId { get; set; }

        [ProtoMember(2)]
        public string InitiatorUnitId { get; set; }

        [ProtoMember(3)]
        public string StatType { get; set; }

        [ProtoMember(4)]
        public int DC { get; set; }
    }
}
