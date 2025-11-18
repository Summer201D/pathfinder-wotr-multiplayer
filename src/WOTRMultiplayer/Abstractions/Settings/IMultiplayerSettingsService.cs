using WOTRMultiplayer.Settings;

namespace WOTRMultiplayer.Abstractions.Settings
{
    public interface IMultiplayerSettingsService
    {
        MultiplayerSettings GetSettings();
    }
}
