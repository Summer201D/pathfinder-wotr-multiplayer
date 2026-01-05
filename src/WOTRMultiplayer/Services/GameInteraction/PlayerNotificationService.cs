using Kingmaker;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.UI;
using Kingmaker.UI.Models.Log.Events;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.Unity;

namespace WOTRMultiplayer.Services.GameInteraction
{
    public class PlayerNotificationService : IPlayerNotificationService
    {
        private readonly ILogger<PlayerNotificationService> _logger;
        private readonly IMainThreadAccessor _mainThreadAccessor;

        public PlayerNotificationService(
            ILogger<PlayerNotificationService> logger,
            IMainThreadAccessor mainThreadAccessor)
        {
            _logger = logger;
            _mainThreadAccessor = mainThreadAccessor;
        }

        public void ShowModalMessage(string messageKey, params object[] args)
        {
            _mainThreadAccessor.Post(() =>
            {
                var message = GetLocalizedText(messageKey, args);
                EventBus.RaiseEvent<IMessageModalUIHandler>(x => x.HandleOpen(message, MessageModalBase.ModalType.Message, null));
            });
        }

        public void ShowWarningNotification(string messageKey, params object[] args)
        {
            _mainThreadAccessor.Post(() =>
            {
                var message = GetLocalizedText(messageKey, args);
                EventBus.RaiseEvent<IWarningNotificationUIHandler>(x => x.HandleWarning(message, true), true);
            });
        }

        public void AddCombatText(string messageKey, params object[] args)
        {
            _mainThreadAccessor.Post(() =>
            {
                var message = GetLocalizedText(messageKey, args);
                Game.Instance.GameLogController.AddReadyEvent(new GameLogEventWarningNotification(message));
            });
        }

        private string GetLocalizedText(string messageKey, params object[] args)
        {
            var localizedPart = new LocalizedString { Key = messageKey };
            var message = string.Format(localizedPart, args);
            return message;
        }
    }
}
