using System;
using System.Linq;
using WOTRMultiplayer.Abstractions.MP;

namespace WOTRMultiplayer.MP
{
    public class MultiplayerSettingsProvider : IMultiplayerSettingsProvider
    {
        private MultiplayerSettings _settings;

        public MultiplayerSettings Settings => _settings ??= InitiDefault();

        private MultiplayerSettings InitiDefault()
        {
            return new MultiplayerSettings
            {
                PlayerName = Guid.NewGuid().ToString().Split('-').First(),
                HostPortRangeStart = 1024,
                HostPortRangeEnd = ushort.MaxValue,
                ForcedPauseDefaultTerminationDelay = TimeSpan.FromSeconds(3),
                ForcedPauseRandomEncounterTerminationDelay = TimeSpan.FromSeconds(8),
                EnableCombatAIActionsSync = true
            };
        }
    }
}
