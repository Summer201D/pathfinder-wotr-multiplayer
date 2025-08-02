using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Loot
{
    public class NetworkLootContainer
    {
        public string Id { get; set; }

        public NetworkVector3 Position { get; set; }

        public List<NetworkItem> Items { get; set; } = [];
    }
}
