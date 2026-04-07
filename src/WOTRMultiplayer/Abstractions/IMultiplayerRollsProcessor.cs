using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace WOTRMultiplayer.Abstractions
{
    public interface IMultiplayerRollsProcessor
    {
        int? OnBeforeRuleCalculateDamageRoll(RuleCalculateDamage ruleCalculateDamage, DiceFormula diceFormula);

        bool OnBeforeRollRuleHealDamage(RuleHealDamage ruleHealDamage, DiceFormula diceFormula);

        int? OnBeforeRuleDealStatDamageRoll(RuleDealStatDamage ruleDealStatDamage, DiceFormula damageFormula, int criticalModifier);

        int? OnRollDice(RuleRollDice ruleRollDice);
    }
}
