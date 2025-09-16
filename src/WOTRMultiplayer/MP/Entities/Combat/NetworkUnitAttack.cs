using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Combat
{
    public class NetworkUnitAttack
    {
        public string ExecutorUnitId { get; set; }

        public string TargetUnitId { get; set; }

        public bool IsFullAttack { get; set; }

        public List<NetworkVector3> VectorPath { get; set; } = [];
    }
}
