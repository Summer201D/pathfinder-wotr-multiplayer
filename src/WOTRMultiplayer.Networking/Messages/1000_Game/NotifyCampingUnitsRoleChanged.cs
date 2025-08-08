using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1036)]
    public class NotifyCampingUnitsRoleChanged
    {
        [ProtoMember(1)]
        public List<NetworkCampingRole> Roles { get; set; } = [];
    }
}
