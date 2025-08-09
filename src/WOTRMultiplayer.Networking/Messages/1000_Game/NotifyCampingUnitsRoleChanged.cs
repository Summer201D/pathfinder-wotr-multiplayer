using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

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
