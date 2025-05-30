using UnityEngine;

namespace WOTRMultiplayer.Abstractions.UI
{
    public interface IPortraitProvider
    {
        Sprite GetPortrait(string name);

        void Initialize();
    }
}
