using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingRaceSelected)]
    public class NotifyLevelingRaceSelected : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public string RaceId { get; set; }
    }
}
