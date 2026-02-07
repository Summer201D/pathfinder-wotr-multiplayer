using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.ClientTogglePauseOff)]
    public class ClientTogglePauseOff
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
