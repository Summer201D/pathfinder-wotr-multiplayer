namespace WOTRMultiplayer.Settings
{
    public class WellKnownSettingKey<T>
    {
        public string Key { get; set; }

        public T DefaultValue { get; private set; }

        public WellKnownSettingKey(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public WellKnownSettingKey<string> AsString()
        {
            var asString = new WellKnownSettingKey<string>(DefaultValue.ToString())
            {
                Key = Key
            };

            return asString;
        }
    }
}
