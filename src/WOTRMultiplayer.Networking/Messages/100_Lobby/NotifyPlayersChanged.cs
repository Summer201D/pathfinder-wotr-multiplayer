using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages._100_Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(104)]
    public class NotifyPlayersChanged
    {
        [ProtoMember(1)]
        public List<NetworkPlayer> Players { get; set; } = [];
    }
}
