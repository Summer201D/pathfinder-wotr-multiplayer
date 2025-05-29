using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(103)]
    public class NotifyGameCharactersChanged
    {
        [ProtoMember(1)]
        public string GameName { get; set; }

        [ProtoMember(2)]
        public List<string> Portraits { get; set; } = [];
    }
}
