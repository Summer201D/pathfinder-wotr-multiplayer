using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Content
{
    public class NetworkDLC
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsAvailable { get; set; }

        public List<NetworkDLCReward> Rewards { get; set; } = [];
    }
}
