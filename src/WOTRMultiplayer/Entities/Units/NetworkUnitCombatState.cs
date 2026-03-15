using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Units
{
    public class NetworkUnitCombatState
    {
        public List<NetworkUnitEngagement> EngagedUnits { get; set; } = [];

        public List<NetworkUnitEngagement> EngagedBy { get; set; } = [];

        public bool NotSurprised { get; set; }
    }
}
