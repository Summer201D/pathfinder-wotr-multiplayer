using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(10012)]
    public class NotifyCombatStarted
    {
        [ProtoMember(1)]
        public List<NetworkUnit> Units { get; set; } = [];
    }
}
