using System.Collections.Generic;
using Kingmaker.EntitySystem.Stats;

namespace WOTRMultiplayer.MP.Entities.Rolls
{
    public class PartyStatCheckRoll : NetworkDiceRollBase
    {
        public int DifficultyClass { get; set; }

        public StatType StatType { get; set; }

        public PartyStatCheckRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        protected override IEnumerable<string> GetUniquinessIdentifiers()
        {
            yield return DifficultyClass.ToString();
            yield return StatType.ToString();
        }
    }
}
