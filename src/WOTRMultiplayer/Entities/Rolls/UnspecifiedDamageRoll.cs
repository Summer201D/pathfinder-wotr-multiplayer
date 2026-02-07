using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    /// <summary>
    /// Direct damage with no reason (attack / ability)
    /// e.g. Demon in market basement
    /// </summary>
    public class UnspecifiedDamageRoll : NetworkDiceRollBase
    {
        public string TargetId { get; set; }

        public UnspecifiedDamageRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        public override IEnumerable<string> GetUniquinessIdentifiers()
        {
            return [TargetId];
        }
    }
}
