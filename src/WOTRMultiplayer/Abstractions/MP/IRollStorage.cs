using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IRollStorage
    {
        void Add(RollDice rollDice);

        RollDice Get(int rollId, long playerId);

        int GetUniqueId(RollDice roll);

        void Reset();
    }
}
