namespace WOTRMultiplayer.MP.Entities.Rolls
{
    public class NetworkDamageValueRoll
    {
        public float TacticalCombatDRModifier { get; set; }

        public int? MaximumDamage { get; set; }

        public int ValueWithoutReduction { get; set; }

        public int RollAndBonusValue { get; set; }

        public int RollResult { get; set; }
    }
}
