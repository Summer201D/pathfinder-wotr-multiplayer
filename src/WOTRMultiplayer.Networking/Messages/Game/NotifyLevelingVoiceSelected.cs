using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingVoiceSelected)]
    public class NotifyLevelingVoiceSelected
    {
        [ProtoMember(1)]
        public NetworkLevelingVoice Voice { get; set; }
    }
}
