using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;

namespace WOTRMultiplayer.Abstractions
{
    public interface IMultiplayerRollsProcessor
    {
        int? OnBeforeRuleCalculateDamageRoll(RuleCalculateDamage ruleCalculateDamage, DiceFormula diceFormula);

        bool OnBeforeRuleAttackOvercomeConcealmentRoll(RuleAttackRoll ruleAttackRoll);

        bool OnBeforeRuleAttackFortificationRoll(RuleAttackRoll ruleAttackRoll);

        bool OnBeforeRuleAttackRoll(RuleAttackRoll ruleAttackRoll);

        bool OnBeforeRuleSavingThrowRoll(RuleSavingThrow ruleSavingThrow);

        bool OnBeforeRuleSpellResistanceCheckRoll(RuleSpellResistanceCheck ruleSpellResistanceCheck);

        bool OnBeforeRuleSkillCheckRoll(RuleSkillCheck ruleSkillCheck);

        bool OnBeforeRuleInitiativeRoll(RuleInitiativeRoll ruleInitiativeRoll);

        bool OnBeforeRuleCheckConcentrationRoll(RuleCheckConcentration ruleCheckConcentration);

        bool OnBeforeRuleConcealmentCheckTrigger(RuleConcealmentCheck ruleConcealmentCheck);

        bool OnBeforeParryDataTrigger(RuleAttackRoll.ParryData parryData);

        bool OnBeforeRuleDispelMagicRoll(RuleDispelMagic ruleDispelMagic);

        bool OnBeforeRuleCheckCastingDefensivelyRoll(RuleCheckCastingDefensively ruleCheckCastingDefensively);

        bool OnBeforeRollRuleHealDamage(RuleHealDamage ruleHealDamage, bool isTacticalCombat, DiceFormula diceFormula);

        bool OnBeforeRuleCastSpellRoll(RuleCastSpell ruleCastSpell, bool isSpellFailure);

        bool OnBeforeRuleEnterStealthRoll(RuleEnterStealth ruleEnterStealth);

        void OnBeforeRuleRollChanceTrigger(RuleRollChance ruleRollChance);

        int? OnBeforeRuleDealStatDamageRoll(RuleDealStatDamage ruleDealStatDamage, DiceFormula damageFormula, int criticalModifier);

        bool OnBeforeRuleDrainEnergyRoll(RuleDrainEnergy ruleDrainEnergy, RuleRollDice rollD20);

        bool OnBeforeRuleCombatManeuverRoll(RuleCombatManeuver ruleCombatManeuver);
    }
}
