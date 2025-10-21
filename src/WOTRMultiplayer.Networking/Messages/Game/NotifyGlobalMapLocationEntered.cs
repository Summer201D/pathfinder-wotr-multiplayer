using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapLocationEntered)]
    public class NotifyGlobalMapLocationEntered
    {
        [ProtoMember(1)]
        public NetworkGlobalMapLocation Location { get; set; }
    }
}
