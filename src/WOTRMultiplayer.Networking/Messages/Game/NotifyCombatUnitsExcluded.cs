using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatUnitsExcluded)]
    public class NotifyCombatUnitsExcluded
    {
        [ProtoMember(1)]
        [LogMe]
        public List<string> Units { get; set; } = [];
    }
}
