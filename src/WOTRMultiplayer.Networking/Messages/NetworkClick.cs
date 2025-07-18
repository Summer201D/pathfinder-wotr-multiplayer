using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkClick
    {
        [ProtoMember(1)]
        public string TargetUnitId { get; set; }

        [ProtoMember(2)]
        public int Button { get; set; }

        [ProtoMember(3)]
        public bool MuteEvents { get; set; }

        [ProtoMember(4)]
        public string SelectedUnitId { get; set; }

        [ProtoMember(5)]
        public float WorldPositionX { get; set; }

        [ProtoMember(6)]
        public float WorldPositionY { get; set; }

        [ProtoMember(7)]
        public float WorldPositionZ { get; set; }
    }
}
