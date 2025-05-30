using System.Collections.Concurrent;
using System.Collections.Immutable;
using Kingmaker.BundlesLoading;
using UnityEngine;
using WOTRMultiplayer.Abstractions.UI;

namespace WOTRMultiplayer.UI
{
    public class ResourceLibraryPortraitLoader : IPortraitProvider
    {
        private ImmutableDictionary<string, Sprite> _portraits;

        public Sprite GetPortrait(string name)
        {
            _portraits.TryGetValue(name, out var sprite);

            return sprite;
        }

        public void Initialize()
        {
            _portraits = LoadAssets();
        }

        private ImmutableDictionary<string, Sprite> LoadAssets()
        {
            var bundle = BundlesLoadService.Instance.RequestBundle("portraits");
            // had no success to limit loading
            var allPortraits = bundle.LoadAllAssets<Sprite>();
            var characterPortraits = new ConcurrentDictionary<string, Sprite>();
            for (int i = 0; i < allPortraits.Length; i++)
            {
                var portrait = allPortraits[i];
                if (!string.IsNullOrEmpty(portrait.name) && portrait.name.EndsWith("Portrait", System.StringComparison.OrdinalIgnoreCase))
                {
                    characterPortraits.TryAdd(portrait.name, portrait);
                    continue;
                }

                // freeup memory
                UnityEngine.Object.DestroyImmediate(portrait);
            }

            return characterPortraits.ToImmutableDictionary();
        }
    }
}
