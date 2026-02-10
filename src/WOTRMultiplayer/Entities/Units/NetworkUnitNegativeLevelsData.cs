using System;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;

namespace WOTRMultiplayer.Entities.Units
{
    public class NetworkUnitNegativeLevelsData
    {
        public int Count { get; set; }

        public SavingThrowType SavingThrowType { get; set; }

        public EnergyDrainType EnergyDrainType { get; set; }

        public TimeSpan? Duration { get; set; }
    }
}
