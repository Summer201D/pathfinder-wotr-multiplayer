using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1007)]
    public class NotifyCampingStateChanged
    {
        [ProtoMember(1)]
        public NetworkCampingState State { get; set; }
    }
}
