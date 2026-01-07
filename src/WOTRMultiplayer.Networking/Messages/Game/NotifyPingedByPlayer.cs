using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyPingedByPlayer)]
    public class NotifyPingedByPlayer
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }

        [ProtoMember(2)]
        public NetworkPing Ping { get; set; }
    }
}
