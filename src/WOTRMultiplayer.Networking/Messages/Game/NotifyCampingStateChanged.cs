using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCampingStateChanged)]
    public class NotifyCampingStateChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkCampingState State { get; set; }
    }
}
