using Kingmaker.RuleSystem;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.GameInteraction.CombatLog;
using WOTRMultiplayer.Abstractions.GameInteraction.CombatLog.Tooltips;

namespace WOTRMultiplayer.Playground.Core.Dummies
{
    public class DummyPlayerNotificationService : IPlayerNotificationService
    {
        public void AddCombatText(string messageKey, CombatTextSeverity combatTextSeverity, params object[] args)
        {
        }

        public void AddCombatText(RulebookEvent rulebookEvent)
        {
        }

        public void AddCombatText(string messageKey, CombatTextSeverity combatTextSeverity, AbilityTooltipLog abilityTooltipLog, params object[] args)
        {
        }

        public void ShowModalMessage(string messageKey, params object[] args)
        {
        }

        public void ShowWarningNotification(string messageKey, bool addToLog = true, float warningDuration = 4f, params object[] args)
        {
        }
    }
}
