using ProtoBuf;
using WOTRMultiplayer.Networking.Awaiters;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Requests
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Request.DiceRollValueResponse)]
    public class DiceRollValueResponse : IAwaitableResponse
    {
        [ProtoMember(1)]
        public int RollId { get; set; }

        [ProtoMember(2)]
        public NetworkRollValue RollValue { get; set; }

        [ProtoMember(3)]
        public string UnitId { get; set; }

        [ProtoMember(4)]
        public long PlayerId { get; set; }

        public string GetKey()
        {
            return string.Join(":", PlayerId, UnitId, RollId.ToString());
        }
    }
}
