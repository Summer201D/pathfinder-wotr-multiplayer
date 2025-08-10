using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkForcedPause
    {
        public string Reason { get; set; }

        public HashSet<long> ReadyPlayers { get; set; } = [];
    }
}
