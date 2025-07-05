using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1008)]
    public class CueWitnessed
    {
        [ProtoMember(1)]
        public string CueName { get; set; }

        [ProtoMember(2)]
        public string DialogName { get; set; }
    }
}
