using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkKingdomEntryPoint
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public NetworkKingdomSettlement Settlement { get; set; }
    }
}
