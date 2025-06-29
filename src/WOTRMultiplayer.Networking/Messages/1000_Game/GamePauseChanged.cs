using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1004)]
    public class GamePauseChanged
    {
        [ProtoMember(1)]
        public bool IsPaused { get; set; }
    }
}
