using System;
using System.Collections.Generic;

namespace WOTRMultiplayer.Entities
{
    public class NetworkForcedPause
    {
        public NetworkForcedPauseReason Reason { get; set; }

        public HashSet<long> ReadyPlayers { get; set; } = [];

        public TimeSpan? RemovalDelay { get; set; }

        public bool IsLifting { get; set; }
    }
}
