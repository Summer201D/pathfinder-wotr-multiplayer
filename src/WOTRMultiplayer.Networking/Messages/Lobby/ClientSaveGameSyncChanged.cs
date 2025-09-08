using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Lobby.ClientSaveGameSyncChanged)]
    public class ClientSaveGameSyncChanged
    {
        [ProtoMember(1)]
        public string Status { get; set; }
    }
}
