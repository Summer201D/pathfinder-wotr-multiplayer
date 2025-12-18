using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingRaceSelected)]
    public class NotifyLevelingRaceSelected
    {
        [ProtoMember(1)]
        public string RaceId { get; set; }
    }
}
