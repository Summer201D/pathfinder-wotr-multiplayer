using Kingmaker.RuleSystem;
using WOTRMultiplayer.Abstractions.GameInteraction.CombatLog;
using WOTRMultiplayer.Abstractions.GameInteraction.CombatLog.Tooltips;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IPlayerNotificationService
    {
        void ShowModalMessage(string messageKey, params object[] args);

        void ShowWarningNotification(string messageKey, bool addToLog = true, params object[] args);

        void AddCombatText(string messageKey, CombatTextSeverity combatTextSeverity, params object[] args);

        void AddCombatText(string messageKey, CombatTextSeverity combatTextSeverity, AbilityTooltipLog abilityTooltipLog, params object[] args);

        void AddCombatText(RulebookEvent rulebookEvent);
    }
}
