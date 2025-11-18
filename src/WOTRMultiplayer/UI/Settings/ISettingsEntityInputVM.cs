namespace WOTRMultiplayer.UI.Settings
{
    public interface ISettingsEntityInputVM
    {
        bool IsValid { get; }

        void RevertTempValue();

        void RevertToDefault();
    }
}
