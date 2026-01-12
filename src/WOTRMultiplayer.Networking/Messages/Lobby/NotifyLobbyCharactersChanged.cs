using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Lobby.NotifyLobbyCharactersChanged)]
    public class NotifyLobbyCharactersChanged
    {
        [ProtoMember(1)]
        public List<NetworkCharacter> Characters { get; set; } = [];
    }
}
