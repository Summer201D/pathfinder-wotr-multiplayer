using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyInvalidCombatTurnStarted)]
    public class NotifyInvalidCombatTurnStarted
    {
        [ProtoMember(1)]
        [LogMe]
        public string UnitId { get; set; }
    }
}
