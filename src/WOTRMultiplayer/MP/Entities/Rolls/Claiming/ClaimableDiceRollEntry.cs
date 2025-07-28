using System.Collections.Concurrent;
using System.Linq;
using WOTRMultiplayer.MP.Entities.Rolls.Claiming.Values;

namespace WOTRMultiplayer.MP.Entities.Rolls.Claiming
{
    public class ClaimableDiceRollEntry
    {
        public ConcurrentQueue<ClaimableDiceRollValue<RollValueBase>> Rolls { get; set; } = new();

        public bool IsClaimed => Rolls.All(r => r.IsClaimed);
    }
}
