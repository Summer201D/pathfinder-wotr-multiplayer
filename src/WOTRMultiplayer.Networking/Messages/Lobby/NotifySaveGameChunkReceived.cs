using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Lobby.NotifySaveGameChunkReceived)]
    public class NotifySaveGameChunkReceived
    {
        [ProtoMember(1)]
        public int ChunkNumber { get; set; }
    }
}
