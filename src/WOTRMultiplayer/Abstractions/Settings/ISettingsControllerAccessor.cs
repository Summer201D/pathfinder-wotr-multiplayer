using System;
using WOTRMultiplayer.Settings;

namespace WOTRMultiplayer.Abstractions.Settings
{
    public interface ISettingsControllerAccessor
    {
        T GetValue<T>(WellKnownSettingKey<T> key);

        TimeSpan GetTimeSpanValue(WellKnownSettingKey<TimeSpan> key);
    }
}
