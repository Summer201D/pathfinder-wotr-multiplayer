using System.Linq;
using TMPro;
using WOTRMultiplayer.Abstractions.GameInteraction;

namespace WOTRMultiplayer.Services.GameInteraction
{
    public class UISyncCountersService : IUISyncCountersService
    {
        public void UpdateButtonTextCounter(TextMeshProUGUI buttonText, int readyPlayersCount, int totalPlayersCount)
        {
            var baseText = GetButtonTextWithoutCounter(buttonText);
            baseText += $" ({readyPlayersCount}/{totalPlayersCount})";
            buttonText.SetText(baseText);
        }

        public void RemoveButtonTextCounter(TextMeshProUGUI buttonText)
        {
            var baseText = GetButtonTextWithoutCounter(buttonText);
            buttonText.SetText(baseText);
        }

        private string GetButtonTextWithoutCounter(TextMeshProUGUI buttonText)
        {
            var baseText = buttonText.text;
            if (baseText != null && baseText.EndsWith(")"))
            {
                var parts = baseText.Split(' ');
                baseText = string.Join(" ", parts.Take(parts.Length - 1));
            }

            return baseText ?? string.Empty;
        }
    }
}
