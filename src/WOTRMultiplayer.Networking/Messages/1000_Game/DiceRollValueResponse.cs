using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1007)]
    public class DiceRollValueResponse
    {
        [ProtoMember(1)]
        public int RollId { get; set; }

        [ProtoMember(2)]
        public NetworkRollValue RollValue { get; set; }
    }
}
