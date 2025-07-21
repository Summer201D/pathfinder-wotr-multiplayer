using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkVector3
    {
        [ProtoMember(1)]
        public float X { get; set; }

        [ProtoMember(2)]
        public float Y { get; set; }

        [ProtoMember(3)]
        public float Z { get; set; }

        public NetworkVector3()
        {
        }

        public NetworkVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"<{X},{Y},{Z}>";
        }
    }
}
