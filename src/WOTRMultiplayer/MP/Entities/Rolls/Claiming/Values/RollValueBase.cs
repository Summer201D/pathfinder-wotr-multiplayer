using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Rolls.Claiming.Values
{
    public abstract class RollValueBase
    {
        public List<int> RollHistory { get; set; } = [];

        public object Value { get; set; }
    }
}
