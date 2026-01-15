using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkGlobalMapKingdomSettlement
    {
        [ProtoMember(1)]
        public string Id { get; set; }
    }
}
