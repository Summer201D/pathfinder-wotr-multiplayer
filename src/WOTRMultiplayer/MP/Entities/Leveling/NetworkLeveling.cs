using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Leveling
{
    public class NetworkLeveling
    {
        public string UnitId { get; set; }

        public HashSet<long> PlayerReadiness { get; set; } = [];

        public int PhaseIndex { get; set; }

        public NetworkLeveling(string unitId)
        {
            UnitId = unitId;
        }
    }
}
