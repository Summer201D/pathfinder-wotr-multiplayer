using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Combat
{
    public class NetworkUnitLootUnit
    {
        public string InitiatorUnitId { get; set; }

        public string TargetUnitId { get; set; }

        public List<NetworkVector3> VectorPath { get; set; }

        public string MovementLimit { get; set; }
    }
}
