using System.Collections.Concurrent;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkRandomEncounter
    {
        public ConcurrentDictionary<string, int> SpecialEncounters { get; set; } = [];

        public float HoursPassedBeforeEncounter { get; set; }

        public int GuardSlotRoll { get; set; }

        public int CamouflageRoll { get; set; }

        public int RandomUnitSeed { get; set; }

        public float PlaceUnitsInCampSharedYRoll { get; set; }

        public ConcurrentDictionary<string, float> PlaceUnitsInCampUnitYRolls { get; set; } = [];

        public ConcurrentDictionary<string, float> PlaceUnitsInCampUnitEndPositionRolls { get; set; } = [];

        public float PlaceUnitsOutsideOfCampSharedYRoll { get; set; }

        public ConcurrentDictionary<string, float> PlaceUnitsOutsideOfCampUnitYRolls { get; set; } = [];

        public ConcurrentDictionary<string, float> PlaceUnitsOutsideOfCampUnitEndPositionRolls { get; set; } = [];
    }
}
