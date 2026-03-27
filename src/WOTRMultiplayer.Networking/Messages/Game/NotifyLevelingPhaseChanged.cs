using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingPhaseChanged)]
    public class NotifyLevelingPhaseChanged : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkLevelingPhase Phase { get; set; }
    }
}
