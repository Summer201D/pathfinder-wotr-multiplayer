using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkTargetWrapper
    {
        [ProtoMember(1)]
        public NetworkVector3 Point { get; set; }

        [ProtoMember(2)]
        public float? Orientation { get; set; }

        [ProtoMember(3)]
        public string UnitUniqueId { get; set; }
    }
}
