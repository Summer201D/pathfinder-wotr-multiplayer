using System;
using Kingmaker.Settings;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.Settings;

namespace WOTRMultiplayer.Settings
{
    public class SettingsControllerAccessor : ISettingsControllerAccessor
    {
        private readonly ILogger<SettingsControllerAccessor> _logger;

        public SettingsControllerAccessor(ILogger<SettingsControllerAccessor> logger)
        {
            _logger = logger;
        }

        public T GetValue<T>(WellKnownSettingKey<T> setting)
        {
            var value = GetValue(setting.Key, setting.DefaultValue);
            return value;
        }

        public TimeSpan GetTimeSpanValue(WellKnownSettingKey<TimeSpan> key)
        {
            var value = GetValue<string>(key.Key, null);
            if (string.IsNullOrEmpty(value) || !TimeSpan.TryParse(value, out var timeSpanValue))
            {
                return key.DefaultValue;
            }

            return timeSpanValue;
        }

        private T GetValue<T>(string key, T defaultValue)
        {
            try
            {
                var value = SettingsController.GeneralSettingsProvider.GetValue<T>(key, defaultValue);

                return value;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogWarning(ex, "Requested setting doesn't exist. Using default value. Key={Key}, DefaultValue={DefaultValue}", key, defaultValue);
                return defaultValue;
            }
        }
    }
}
