using Kingmaker.UnitLogic.Abilities;

namespace WOTRMultiplayer.Abstractions.GameInteraction.CombatLog.Tooltips
{
    public class AbilityTooltipLog
    {
        public AbilityData AbilityData { get; set; }

        public AbilityTooltipLog(AbilityData ability)
        {
            AbilityData = ability;
        }
    }
}
