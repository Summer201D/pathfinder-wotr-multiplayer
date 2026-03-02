using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;

namespace WOTRMultiplayer.Abstractions.GameInteraction.CombatLog
{
    public class AbilityLogParameter : ColorizedParameter
    {
        public AbilityLogParameter(string value)
            : base(value, LogThreadBase.Strings.UseAbility.Color)
        {
        }
    }
}
