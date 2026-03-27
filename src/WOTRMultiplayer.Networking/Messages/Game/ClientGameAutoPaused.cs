using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.ClientGameAutoPaused)]
    public class ClientGameAutoPaused
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkForcedPause Pause { get; set; }
    }
}
