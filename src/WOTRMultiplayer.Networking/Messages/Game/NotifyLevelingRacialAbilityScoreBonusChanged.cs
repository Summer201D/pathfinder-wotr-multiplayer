using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingRacialAbilityScoreBonusChanged)]
    public class NotifyLevelingRacialAbilityScoreBonusChanged
    {
        [ProtoMember(1)]
        public string Direction { get; set; }
    }
}
