namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IPlayerNotificationService
    {
        void ShowModalMessage(string messageKey, params object[] args);

        void ShowWarningNotification(string messageKey, params object[] args);

        void AddCombatText(string messageKey, params object[] args);
    }
}
