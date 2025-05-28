using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(103)]
    public class NotifySaveChanged
    {
        [ProtoMember(1)]
        public string SaveGameName { get; set; }

        [ProtoMember(2)]
        public List<string> Portraits { get; set; } = new();
    }
}
