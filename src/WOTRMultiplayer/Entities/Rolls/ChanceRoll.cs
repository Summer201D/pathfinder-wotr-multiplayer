using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    public class ChanceRoll : NetworkDiceRollBase
    {
        public int Chance { get; set; }

        public string Type { get; set; }

        public ChanceRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        public override IEnumerable<string> GetUniquinessIdentifiers()
        {
            return [Chance.ToString(), Type];
        }
    }
}
