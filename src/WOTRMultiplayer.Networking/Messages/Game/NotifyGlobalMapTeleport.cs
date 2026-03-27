using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapTeleport)]
    public class NotifyGlobalMapTeleport
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkGlobalMapLocation Location { get; set; }
    }
}
