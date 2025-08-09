using System.Collections.Concurrent;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkRandomEncounter
    {
        [ProtoMember(1)]
        public ConcurrentDictionary<string, int> SpecialEncounters { get; set; } = [];

        [ProtoMember(2)]
        public float HoursPassedBeforeEncounter { get; set; }

        [ProtoMember(3)]
        public int GuardSlotRoll { get; set; }

        [ProtoMember(4)]
        public int CamouflageRoll { get; set; }

        [ProtoMember(5)]
        public int RandomUnitSeed { get; set; }

        [ProtoMember(6)]
        public float PlaceUnitsInCampSharedYRoll { get; set; }

        [ProtoMember(7)]
        public ConcurrentDictionary<string, float> PlaceUnitsInCampUnitYRolls { get; set; } = [];

        [ProtoMember(8)]
        public ConcurrentDictionary<string, float> PlaceUnitsInCampUnitEndPositionRolls { get; set; } = [];

        [ProtoMember(9)]
        public float PlaceUnitsOutsideOfCampSharedYRoll { get; set; }

        [ProtoMember(10)]
        public ConcurrentDictionary<string, float> PlaceUnitsOutsideOfCampUnitYRolls { get; set; } = [];

        [ProtoMember(11)]
        public ConcurrentDictionary<string, float> PlaceUnitsOutsideOfCampUnitEndPositionRolls { get; set; } = [];
    }
}
