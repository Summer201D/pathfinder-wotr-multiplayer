using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapTravelStarted)]
    public class NotifyGlobalMapTravelStarted
    {
        [ProtoMember(1)]
        public NetworkGlobalMapLocation Destination { get; set; }
    }
}
