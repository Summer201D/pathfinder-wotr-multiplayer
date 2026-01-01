using System.Collections.Generic;
using WOTRMultiplayer.Entities.Equipment;

namespace WOTRMultiplayer.Entities.Items
{
    public class NetworkItemsTransfer
    {
        public List<NetworkItem> Items { get; set; }

        public NetworkLootableEntity Source { get; set; }

        public NetworkLootableEntity Destination { get; set; }
    }
}
