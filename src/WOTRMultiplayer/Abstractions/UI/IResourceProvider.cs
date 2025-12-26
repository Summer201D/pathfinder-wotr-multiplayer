using UnityEngine;

namespace WOTRMultiplayer.Abstractions.UI
{
    public interface IResourceProvider
    {
        Sprite GetSprite(string bundleName, string spriteName);

        Texture2D GetTexture2D(string bundleName, string textureName);

        void Initialize();
    }
}
