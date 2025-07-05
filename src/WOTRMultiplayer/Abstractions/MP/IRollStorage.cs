using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IRollStorage
    {
        void Add(NetworkDiceRoll rollDice);

        NetworkDiceRoll Get(int rollId, long playerId);

        int GetUniqueId(NetworkDiceRoll roll);

        void Reset();
    }
}
