using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    public class FortificationAttackRoll : NetworkDiceRollBase
    {
        public int FortificationChance { get; set; }

        public AttackRoll AttackRoll { get; set; }

        public FortificationAttackRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        protected override IEnumerable<string> GetRollIdentifier()
        {
            var attackIdentifier = AttackRoll == null ? null : string.Join(IdSeparator, AttackRoll.GetIdentifier());

            return ["@", FortificationChance.ToString(), "@", attackIdentifier];
        }
    }
}
