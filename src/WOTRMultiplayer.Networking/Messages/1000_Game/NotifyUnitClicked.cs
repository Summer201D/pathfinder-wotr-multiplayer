using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1018)]
    public class NotifyUnitClicked
    {
        [ProtoMember(1)]
        public NetworkClick Click { get; set; }
    }
}
