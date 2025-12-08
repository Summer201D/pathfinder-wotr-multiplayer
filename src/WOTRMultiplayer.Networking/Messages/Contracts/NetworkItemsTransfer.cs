using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkItemsTransfer
    {
        [ProtoMember(1)]
        public List<NetworkItem> Items { get; set; }

        [ProtoMember(2)]
        public NetworkLootableEntity Source { get; set; }

        [ProtoMember(3)]
        public NetworkLootableEntity Destination { get; set; }
    }
}
