using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingVoiceSelected)]
    public class NotifyLevelingVoiceSelected : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkLevelingVoice Voice { get; set; }
    }
}
