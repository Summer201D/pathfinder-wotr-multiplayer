using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatDataCollected)]
    public class NotifyCombatDataCollected
    {
        [ProtoMember(1)]
        [LogMe]
        public long PlayerId { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public List<string> Units { get; set; } = [];
    }
}
