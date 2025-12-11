using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkStealthPerceptionCheck
    {
        [ProtoMember(1)]
        public string InitiatorId { get; set; }

        [ProtoMember(2)]
        public string StealthedUnitId { get; set; }

        [ProtoMember(3)]
        public int Roll { get; set; }

        [ProtoMember(4)]
        public bool IsSuccess { get; set; }

        [ProtoMember(5)]
        public int DC { get; set; }

        [ProtoMember(6)]
        public bool IsTargetInvisible { get; set; }

        [ProtoMember(7)]
        public bool IgnoreDifficultyBonusToDC { get; set; }
    }
}
