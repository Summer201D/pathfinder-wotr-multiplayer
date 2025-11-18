using System;

namespace WOTRMultiplayer.MP.Entities.Settings
{
    public class NetworkMultiplayerSettings
    {
        public string PlayerName { get; set; }

        public int HostPortRangeStart { get; set; }

        public int HostPortRangeEnd { get; set; }

        public TimeSpan ForcedPauseDefaultTerminationDelay { get; set; }

        public TimeSpan ForcedPauseRandomEncounterTerminationDelay { get; set; }

        public bool SyncAICombatActions { get; set; }
    }
}
