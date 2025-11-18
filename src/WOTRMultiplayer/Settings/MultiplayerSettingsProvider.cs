using WOTRMultiplayer.Abstractions.Settings;

namespace WOTRMultiplayer.Settings
{
    public class MultiplayerSettingsProvider : IMultiplayerSettingsService
    {
        private readonly ISettingsControllerAccessor _settingsControllerAccessor;

        public MultiplayerSettingsProvider(ISettingsControllerAccessor settingsControllerAccessor)
        {
            _settingsControllerAccessor = settingsControllerAccessor;
        }

        public MultiplayerSettings GetSettings()
        {
            var settings = new MultiplayerSettings
            {
                PlayerName = _settingsControllerAccessor.GetValue(WellKnownSettings.General.PlayerName),
                SyncAICombatActions = _settingsControllerAccessor.GetValue(WellKnownSettings.Combat.AISync),
                HostPortRangeStart = _settingsControllerAccessor.GetValue(WellKnownSettings.Networking.HostPortRangeStart),
                HostPortRangeEnd = _settingsControllerAccessor.GetValue(WellKnownSettings.Networking.HostPortRangeEnd),
                ForcedPauseDefaultTerminationDelay = _settingsControllerAccessor.GetTimeSpanValue(WellKnownSettings.DangerZone.DefaultForcedPauseTimeout),
                ForcedPauseRandomEncounterTerminationDelay = _settingsControllerAccessor.GetTimeSpanValue(WellKnownSettings.DangerZone.RestEncounterForcedPauseTimeout),
            };

            return settings;
        }
    }
}
