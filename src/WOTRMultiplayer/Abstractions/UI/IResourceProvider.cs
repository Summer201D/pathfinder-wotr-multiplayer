using UnityEngine;

namespace WOTRMultiplayer.Abstractions.UI
{
    public interface IResourceProvider
    {
        Sprite GetPortrait(string name);

        Sprite GetUISprite(string name);

        void Initialize();
    }
}
