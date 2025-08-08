using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1035)]
    public class NotifyCampingStateChanged
    {
        [ProtoMember(1)]
        public NetworkCampingState State { get; set; }
    }
}
