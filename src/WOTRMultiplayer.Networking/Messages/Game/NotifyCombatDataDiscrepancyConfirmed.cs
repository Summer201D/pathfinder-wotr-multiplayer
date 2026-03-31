using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatDataDiscrepancyConfirmed)]
    public class NotifyCombatDataDiscrepancyConfirmed
    {
        [ProtoMember(1)]
        [LogMe]
        public List<string> UnavailableUnits { get; set; } = [];
    }
}
