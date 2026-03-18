using System;
using System.Collections.Generic;
using Kingmaker.Settings;
using Microsoft.Extensions.Logging;
using UnityEngine;
using WOTRMultiplayer.Abstractions;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.Hotkeys;
using WOTRMultiplayer.Abstractions.Settings;
using WOTRMultiplayer.Abstractions.UI.Controllers;
using WOTRMultiplayer.Services.Settings;

namespace WOTRMultiplayer.Services.Hotkeys
{
    public class HotkeysService : IHotkeysService
    {
        private readonly ILogger<HotkeysService> _logger;
        private readonly IMultiplayerActorAccessor _multiplayerActorAccessor;
        private readonly ISettingsControllerAccessor _settingsControllerAccessor;
        private readonly IKeyboardAccessor _keyboardAccessor;
        private readonly IPingInteractionService _pingInteractionService;
        private readonly ILobbyWindowController _lobbyWindowController;
        private readonly List<IDisposable> _bindings = [];

        public HotkeysService(
            ILogger<HotkeysService> logger,
            IMultiplayerActorAccessor multiplayerActorAccessor,
            ISettingsControllerAccessor settingsControllerAccessor,
            IPingInteractionService pingInteractionService,
            ILobbyWindowController lobbyWindowController,
            IKeyboardAccessor keyboardAccessor)
        {
            _logger = logger;
            _multiplayerActorAccessor = multiplayerActorAccessor;
            _settingsControllerAccessor = settingsControllerAccessor;
            _keyboardAccessor = keyboardAccessor;
            _pingInteractionService = pingInteractionService;
            _lobbyWindowController = lobbyWindowController;
        }

        public void Initialize()
        {
            _logger.LogInformation("Initializing hotkeys");

            foreach (var disposable in _bindings)
            {
                disposable?.Dispose();
            }

            _bindings.Clear();

            ConfigureHotkey(WellKnownSettings.Hotkeys.Ping, OnPingHotkey);
            ConfigureHotkey(WellKnownSettings.Hotkeys.ShowLobby, OnShowLobbyHotkey);
            ConfigureHotkey(WellKnownSettings.Hotkeys.ForceUnpause, OnForceUnpauseHotkey);
            ConfigureHotkey(WellKnownSettings.Hotkeys.ForceCombatEnd, OnForceCombatEndHotkey);
        }

        public void ConfigureHotkey(WellKnownSettingKey<KeyBindingPair> hotkey, Action hotkeyHandler)
        {
            try
            {
                var actualValue = _settingsControllerAccessor.GetValue(hotkey);
                if (actualValue.Binding1.Key != KeyCode.None)
                {
                    _keyboardAccessor.RegisterBinding(hotkey.Key, actualValue.Binding1, actualValue.GameModesGroup, actualValue.TriggerOnHold, isHoldTrigger: false);
                }
                if (actualValue.Binding2.Key != KeyCode.None)
                {
                    _keyboardAccessor.RegisterBinding(hotkey.Key, actualValue.Binding2, actualValue.GameModesGroup, actualValue.TriggerOnHold, isHoldTrigger: false);
                }

                var bind = _keyboardAccessor.Bind(hotkey.Key, () => OnHotkeyPressed(hotkey.Key, hotkeyHandler));
                _bindings.Add(bind);
                _logger.LogInformation("Hotkey has been configured. Key={Key}", hotkey.Key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to configure hotkey setting. Key={Key}", hotkey?.Key);
                throw;
            }
        }

        private void OnHotkeyPressed(string key, Action onHotkey)
        {
            if (_multiplayerActorAccessor.Current == null || _multiplayerActorAccessor.Current.IsInLobby)
            {
                return;
            }

            try
            {
                onHotkey.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while executing hotkey handler. Key={Key}", key);
            }
        }

        private void OnPingHotkey()
        {
            var ping = _pingInteractionService.Get();
            if (ping == null)
            {
                return;
            }

            _multiplayerActorAccessor.Current.OnPing(ping);
        }

        private void OnForceUnpauseHotkey()
        {
            _logger.LogWarning("Forcing to unpause");
            _multiplayerActorAccessor.Current.ForceUnpause();
        }

        private void OnForceCombatEndHotkey()
        {
            _logger.LogWarning("Forcing combat to end");
            _multiplayerActorAccessor.Current.ForceCombatEnd();
        }

        private void OnShowLobbyHotkey()
        {
            _lobbyWindowController.EnsureStandaloneWindowInitialized();

            if (_lobbyWindowController.Window.IsVisible)
            {
                return;
            }

            _lobbyWindowController.Window.Show();
        }
    }
}
