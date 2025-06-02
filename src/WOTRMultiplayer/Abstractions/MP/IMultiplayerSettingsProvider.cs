using WOTRMultiplayer.MP;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerSettingsProvider
    {
        MultiplayerSettings Settings { get; }
    }
}
