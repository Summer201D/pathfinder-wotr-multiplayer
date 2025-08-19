using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1061)]
    public class NotifyLevelingAbilityScoreDecreased
    {
        [ProtoMember(1)]
        public NetworkLevelingAbilityScore AbilityScore { get; set; }
    }
}
