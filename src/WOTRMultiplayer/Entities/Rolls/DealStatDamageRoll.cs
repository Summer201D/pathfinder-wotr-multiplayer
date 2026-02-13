using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    public class DealStatDamageRoll : NetworkDiceRollBase
    {
        public bool HalfBecauseSavingThrow { get; set; }

        public bool Immune { get; set; }

        public bool Maximize { get; set; }

        public bool IsDrain { get; set; }

        public bool Empower { get; set; }

        public int? MinStatScoreAfterDamage { get; set; }

        public string CriticalModifierName { get; set; }

        public int CriticalModifierValue { get; set; }

        public int DiceRolls { get; internal set; }

        public string DiceFormulaType { get; internal set; }

        public DealStatDamageRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        protected override IEnumerable<string> GetRollIdentifier()
        {
            return [DiceRolls.ToString(), DiceFormulaType, HalfBecauseSavingThrow.ToString(), Immune.ToString(), Maximize.ToString(), IsDrain.ToString(), Empower.ToString(), MinStatScoreAfterDamage?.ToString(), CriticalModifierName, CriticalModifierValue.ToString()];
        }
    }
}
