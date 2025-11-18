using System.Collections.Generic;

namespace WOTRMultiplayer.Abstractions.Localization
{
    public interface ILocalizationManagerAccessor
    {
        void UpdateCurrentLocalePack(Dictionary<string, string> translations);
    }
}
