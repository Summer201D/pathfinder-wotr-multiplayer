using System.Collections.Concurrent;

namespace WOTRMultiplayer.MP.Entities.Rolls
{
    public class DiceRollEntry
    {
        public NetworkDiceRoll Roll { get; set; }

        public ConcurrentDictionary<long, int> RetrieveHistory { get; set; } = new();

        public DiceRollEntry(NetworkDiceRoll rollDice)
        {
            Roll = rollDice;
        }
    }
}
