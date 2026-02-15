using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Combat
{
    public class NetworkAIDecisionContext
    {
        public List<NetworkVector3> VectorPath { get; set; } = [];

        public bool BestEnableFiveFootStep { get; set; }

        public List<string> ExpendedActions { get; set; } = [];

        public NetworkVector3 BestDestinationPoint { get; set; }

        public NetworkVector3 DestinationPoint { get; set; }

        public decimal BestScore { get; set; }
    }
}
