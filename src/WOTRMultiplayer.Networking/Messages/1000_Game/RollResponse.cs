using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1007)]
    public class RollResponse
    {
        [ProtoMember(1)]
        public RollDice Roll { get; set; }
    }
}
