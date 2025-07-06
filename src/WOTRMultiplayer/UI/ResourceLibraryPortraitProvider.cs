using System.Collections.Concurrent;
using Kingmaker.BundlesLoading;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.UI;

namespace WOTRMultiplayer.UI
{
    public class ResourceLibraryPortraitProvider : IPortraitProvider
    {
        public const string PlaceholderPortrait = "Mask_Portrait";
        public const string PortraitsBundleName = "portraits";

        private readonly ILogger _logger;
        private ConcurrentDictionary<string, ConcurrentDictionary<string, UnityEngine.Sprite>> _resources;

        public ResourceLibraryPortraitProvider(ILogger<ResourceLibraryPortraitProvider> logger)
        {
            _logger = logger;
        }

        public UnityEngine.Sprite GetPortrait(string name)
        {
            _resources.TryGetValue(PortraitsBundleName, out var portraits);
            if (!portraits.TryGetValue(name, out var sprite))
            {
                _logger.LogWarning("Unable to find requested portrait. Name={portraitName}", name);
                portraits.TryGetValue(PlaceholderPortrait, out sprite);
            }

            return sprite;
        }

        public void Initialize()
        {
            if (_resources == null)
            {
                _resources = new ConcurrentDictionary<string, ConcurrentDictionary<string, UnityEngine.Sprite>>();
                _resources.TryAdd(PortraitsBundleName, LoadAssets(PortraitsBundleName));
            }
        }

        private ConcurrentDictionary<string, UnityEngine.Sprite> LoadAssets(string bundleName)
        {
            var bundle = BundlesLoadService.Instance.RequestBundle(bundleName);
            // had no success to limit loading
            // note: you can't delete (Object->Destroy or DestroyImmediate) redundant sprites as it causes texture errors later on
            var allPortraits = bundle.LoadAllAssets<UnityEngine.Sprite>();
            var characterPortraits = new ConcurrentDictionary<string, UnityEngine.Sprite>();
            for (int i = 0; i < allPortraits.Length; i++)
            {
                var portrait = allPortraits[i];

                characterPortraits.TryAdd(portrait.name, portrait);
            }

            return characterPortraits;
        }
    }
}
