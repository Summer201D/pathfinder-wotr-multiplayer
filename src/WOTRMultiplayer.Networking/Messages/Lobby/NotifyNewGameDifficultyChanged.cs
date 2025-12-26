using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Lobby.NotifyNewGameDifficultyChanged)]
    public class NotifyNewGameDifficultyChanged
    {
        [ProtoMember(1)]
        public string Difficulty { get; set; }
    }
}
