using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkDamageRollValue
    {
        [ProtoMember(1)]
        public float TacticalCombatDRModifier { get; set; }

        [ProtoMember(2)]
        public int? MaximumDamage { get; set; }

        [ProtoMember(3)]
        public int ValueWithoutReduction { get; set; }

        [ProtoMember(4)]
        public int RollAndBonusValue { get; set; }

        [ProtoMember(5)]
        public int RollResult { get; set; }
    }
}
