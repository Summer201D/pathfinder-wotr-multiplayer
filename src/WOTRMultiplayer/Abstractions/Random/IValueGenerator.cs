using System;
using WOTRMultiplayer.Entities;
using WOTRMultiplayer.Services.Random;

namespace WOTRMultiplayer.Abstractions.Random
{
    public interface IValueGenerator
    {
        string GenerateUniqueId(UniqueIdType uniqueIdType, string gameId, string identifier);

        int Range(SeedLifetime seedLifetime, int seed, int minInclusive, int maxExclusive);

        int Range(SeedLifetime seedLifetime, string seed, int minInclusive, int maxExclusive);

        float Range(SeedLifetime seedLifetime, string seed, float minInclusive, float maxExclusive);

        void ResetUniqueIdCounters(string gameId);

        void ResetSeedGenerators(params SeedLifetime[] lifetimes);

        Guid CreateGuid(SeedLifetime area, string seed);

        System.Random GetRandom(SeedLifetime seedLifetime, string seed);

        NetworkVector2 GetRandomUnitCircle(SeedLifetime seedLifetime, string seed);
    }
}
