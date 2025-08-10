using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkRandomEncounter
    {
        [ProtoMember(1)]
        public Dictionary<string, int> SpecialEncounters { get; set; } = [];

        [ProtoMember(2)]
        public float HoursPassedBeforeEncounter { get; set; }

        [ProtoMember(3)]
        public int GuardSlotRoll { get; set; }

        [ProtoMember(4)]
        public int CamouflageRoll { get; set; }

        [ProtoMember(5)]
        public int? RandomUnitSeed { get; set; }

        [ProtoMember(6)]
        public Dictionary<string, float> PlaceUnitsInCampRangedRolls { get; set; } = [];

        [ProtoMember(7)]
        public Dictionary<string, string> PlaceUnitsInCampRangedTargetRolls { get; set; } = [];

        [ProtoMember(8)]
        public Dictionary<string, float> PlaceUnitsInCampUnitYRolls { get; set; } = [];

        [ProtoMember(9)]
        public Dictionary<string, float> PlaceUnitsInCampUnitEndPositionRolls { get; set; } = [];

        [ProtoMember(10)]
        public float PlaceUnitsOutsideOfCampSharedYRoll { get; set; }

        [ProtoMember(11)]
        public Dictionary<string, float> PlaceUnitsOutsideOfCampUnitYRolls { get; set; } = [];

        [ProtoMember(12)]
        public Dictionary<string, float> PlaceUnitsOutsideOfCampUnitEndPositionRolls { get; set; } = [];
    }
}
