using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkActivatableAbility
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public string CasterId { get; set; }

        [ProtoMember(3)]
        public string TargetId { get; set; }

        [ProtoMember(4)]
        public bool IsActive { get; set; }
    }
}
