using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1006)]
    public class RollRequest
    {
        [ProtoMember(1)]
        public int RollId { get; set; }
    }
}
