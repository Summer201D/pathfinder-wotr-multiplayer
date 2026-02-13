using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    public class PartyStatCheckRoll : NetworkDiceRollBase
    {
        public int DifficultyClass { get; set; }

        public string StatType { get; set; }

        public PartyStatCheckRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        protected override IEnumerable<string> GetRollIdentifier()
        {
            return [DifficultyClass.ToString(), StatType];
        }
    }
}
