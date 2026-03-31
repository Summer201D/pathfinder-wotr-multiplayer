using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatStageChanged)]
    public class NotifyCombatStageChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public string Stage { get; set; }
    }
}
