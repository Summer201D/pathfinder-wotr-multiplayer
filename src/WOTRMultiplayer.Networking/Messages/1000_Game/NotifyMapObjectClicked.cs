using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1025)]
    public class NotifyMapObjectClicked
    {
        [ProtoMember(1)]
        public NetworkClick Click { get; set; }
    }
}
