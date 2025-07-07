using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkUnit
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public float PositionX { get; set; }

        [ProtoMember(3)]
        public float PositionY { get; set; }

        [ProtoMember(4)]
        public float PositionZ { get; set; }
    }
}
