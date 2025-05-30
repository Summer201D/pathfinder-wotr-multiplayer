using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(107)]
    public class NotifyCharactersOwnerChanged
    {
        [ProtoMember(1)]
        public List<NetworkCharacterOwner> Owners { get; set; } = [];
    }
}
