using System;
using WOTRMultiplayer.Abstractions.Settings;
using WOTRMultiplayer.Settings;

namespace WOTRMultiplayer.Playground.Core.Dummies
{
    public class DummySettingsControllerAccessor : ISettingsControllerAccessor
    {
        public void CreateDefaultValue<TValue>(WellKnownSettingKey<TValue> settingKey)
        {
        }

        public TimeSpan GetTimeSpanValue(WellKnownSettingKey<TimeSpan> key)
        {
            return key.DefaultValue;
        }

        public T GetValue<T>(WellKnownSettingKey<T> key)
        {
            return key.DefaultValue;
        }
    }
}
