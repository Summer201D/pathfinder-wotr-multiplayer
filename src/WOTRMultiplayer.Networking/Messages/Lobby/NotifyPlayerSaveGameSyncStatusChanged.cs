using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Lobby.NotifyPlayerStartFailed)]
    public class NotifyPlayerSaveGameSyncStatusChanged
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }

        [ProtoMember(2)]
        public string Status { get; set; }
    }
}
