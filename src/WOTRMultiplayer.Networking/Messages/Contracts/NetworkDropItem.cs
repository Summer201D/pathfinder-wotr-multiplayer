using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkDropItem
    {
        [ProtoMember(1)]
        public string OwnerEntityId { get; set; }

        [ProtoMember(2)]
        public NetworkItem Item { get; set; }
    }
}
