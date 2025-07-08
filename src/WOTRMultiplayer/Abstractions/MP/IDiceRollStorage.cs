using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IDiceRollStorage
    {
        bool Add(NetworkDiceRoll rollDice);

        NetworkDiceRoll Get(int rollId, long playerId);

        int GetUniqueId(NetworkDiceRoll roll);

        void Reset();

        void Reset<T>()
            where T : NetworkDiceRoll;
    }
}
