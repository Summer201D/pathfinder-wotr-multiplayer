using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkPing
    {
        [ProtoMember(1)]
        public NetworkVector3 WorldPosition { get; set; }

        [ProtoMember(2)]
        public string UnitId { get; set; }

        [ProtoMember(3)]
        public NetworkMapObject MapObject { get; set; }

        [ProtoMember(4)]
        public string Type { get; set; }
    }
}
