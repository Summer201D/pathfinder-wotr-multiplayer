using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(103)]
    public class NotifyGameCharactersChanged
    {
        [ProtoMember(1)]
        public List<NetworkCharacterOwnership> Characters { get; set; } = [];
    }
}
