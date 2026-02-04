using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkPerceptionCheck
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }

        [ProtoMember(2)]
        public NetworkMapObject MapObject { get; set; }

        [ProtoMember(3)]
        public int Roll { get; set; }
    }
}
