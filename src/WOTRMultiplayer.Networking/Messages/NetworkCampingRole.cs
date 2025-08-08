using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkCampingRole
    {
        [ProtoMember(1)]
        public string RoleType { get; set; }

        [ProtoMember(2)]
        public string PrimaryUnitId { get; set; }

        [ProtoMember(3)]
        public string SecondaryUnitId { get; set; }
    }
}
