using WOTRMultiplayer.Random;

namespace WOTRMultiplayer.Abstractions.Random
{
    public interface IValueGenerator
    {
        string GenerateUniqueId(UniqueIdType uniqueIdType, string gameId, string identifier);

        int Range(int seed, int minInclusive, int maxExclusive);

        void Reset(string gameId);
    }
}
