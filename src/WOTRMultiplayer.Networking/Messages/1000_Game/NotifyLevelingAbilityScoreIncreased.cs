using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1060)]
    public class NotifyLevelingAbilityScoreIncreased
    {
        [ProtoMember(1)]
        public NetworkLevelingAbilityScore AbilityScore { get; set; }
    }
}
