using System.Collections.Generic;
using System.Linq;
using WOTRMultiplayer.MP.Entities.Rolls.Claiming.Values;

namespace WOTRMultiplayer.MP.Entities.Rolls.Claiming
{
    public class ClaimableDiceRollEntry
    {
        public List<ClaimableDiceRollValue<RollValueBase>> Rolls { get; set; } = [];

        public bool IsClaimed => Rolls.All(r => r.IsClaimed);
    }
}
