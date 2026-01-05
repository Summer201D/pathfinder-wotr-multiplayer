using TMPro;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IUISyncCountersService
    {
        void UpdateButtonTextCounter(TextMeshProUGUI buttonText, int readyPlayersCount, int totalPlayersCount);

        void RemoveButtonTextCounter(TextMeshProUGUI buttonText);
    }
}
