using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatDataDiscrepancyDetected)]
    public class NotifyCombatDataDiscrepancyDetected
    {
        [ProtoMember(1)]
        [LogMe]
        public List<string> Units { get; set; } = [];
    }
}
