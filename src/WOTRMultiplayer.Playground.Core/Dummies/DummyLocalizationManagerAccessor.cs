using System;
using System.Collections.Generic;
using WOTRMultiplayer.Abstractions.Localization;

namespace WOTRMultiplayer.Playground.Core.Dummies
{
    public class DummyLocalizationManagerAccessor : ILocalizationManagerAccessor
    {
        public void UpdateCurrentLocalePack(Dictionary<string, string> translations)
        {
            foreach (var kv in translations)
            {
                Console.WriteLine($"Key={kv.Key} Value={kv.Value}");
            }
        }
    }
}
