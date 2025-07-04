using System;
using System.Numerics;
using System.Text;
using System.Threading;
using Kingmaker.Controllers.Clicks.Handlers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.RuleSystem.Rules;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.Abstractions.UI;
using WOTRMultiplayer.Abstractions.UI.Controllers;
using WOTRMultiplayer.Abstractions.UI.Windows;
using WOTRMultiplayer.HarmonyPatches.PubSub;
using WOTRMultiplayer.MP.Entities;
using WOTRMultiplayer.UI.Lobby;

namespace WOTRMultiplayer.MP
{
    public class Multiplayer : IMultiplayer
    {
        private IMultiplayerWindow _multiplayerWindow;
        private ILobbyWindow _lobbyWindow;

        private readonly ILobbyWindowController _lobbyWindowController;
        private readonly IMultiplayerClient _multiplayerClient;
        private readonly IMultiplayerHost _multiplayerHost;
        private readonly ILogger _logger;

        public IUIFactory Factory { get; private set; }

        public bool IsActive => _multiplayerClient.IsActive || _multiplayerHost.IsActive;

        public Multiplayer(
            ILogger<Multiplayer> logger,
            IUIFactory uiFactory,
            ILobbyWindowController lobbyWindowController,
            IMultiplayerHost multiplayerHost,
            IMultiplayerClient multiplayerClient)
        {
            _logger = logger;
            Factory = uiFactory;
            _multiplayerHost = multiplayerHost;
            _multiplayerClient = multiplayerClient;
            _lobbyWindowController = lobbyWindowController;
        }

        public bool InitializeMultiplayer(InitializeMultiplayerContext context)
        {
            if (_multiplayerHost.IsActive)
            {
                _logger.LogWarning("Multiplayer host has not been properly disposed. Verify exit game/main menu handles");
                _multiplayerHost.Dispose();
            }

            if (_multiplayerClient.IsActive)
            {
                _logger.LogWarning("Multiplayer client has not been properly disposed. Verify exit game/main menu handlers");
                _multiplayerClient.Dispose();
            }

            _multiplayerWindow = Factory.InitializeMultiplayerWindow(context, ShowMultiplayerWindow);

            return true;
        }

        public void TerminateMultiplayer()
        {
            _logger.LogInformation("Disposing both multiplayer host/client");
            _multiplayerHost.Dispose();
            _multiplayerClient.Dispose();
            _lobbyWindowController.ResetOwnerContent(LobbyWindowOwner.EscMenu);
            _lobbyWindowController.OnCharacterOwnerChanged = null;
            _logger.LogInformation("Disposing Esc menu window game objects");
            Factory.DestroyLobbyWindow(_lobbyWindow);
        }

        public void InitializeEscMenuLobbyWindow(InitializeEscMenuLobbyWindowContext context)
        {
            _logger.LogInformation("Creating Esc menu lobby item");
            _lobbyWindow = Factory.InitializeEscMenuLobbyWindow(context, _multiplayerHost.IsActive, ShowEscMenuMultiplayerLobby);

            _lobbyWindow.NetworkGame = GetNetworkGame;
            _lobbyWindow.AssignLobbyController(_lobbyWindowController);

            _lobbyWindowController.OnCharacterOwnerChanged = OnLobbyCharacterOwnerChanged;
        }

        public void MoveCharacter(UnitEntityData unit, ClickGroundHandler.CommandSettings settings)
        {
            var destination = new Vector3(settings.Destination.x, settings.Destination.y, settings.Destination.z);
            var multiplayerParticipant = GetMultiplayerParticipant();
            multiplayerParticipant.MoveCharacter(unit.CharacterName, destination, settings.Delay, settings.Orientation);
        }

        public bool CanControlCharacter(string characterName)
        {
            var multiplayerParticipant = GetMultiplayerParticipant();
            return multiplayerParticipant.CanControlCharacter(characterName);
        }

        public bool StartGameMode(GameModeType type)
        {
            var allowedToRun = type != GameModeType.EscMode && type != GameModeType.FullScreenUi;
            _logger.LogInformation("Trying to start GameModeType. Mode={mode}, AllowedToRun={allowedToRun}", type.Name, allowedToRun);

            if (type == GameModeType.Pause)
            {
                var multiplayerParticipant = GetMultiplayerParticipant();
                multiplayerParticipant.Pause();
            }

            return allowedToRun;
        }

        public bool StopGameMode(GameModeType type)
        {
            _logger.LogInformation("Trying to stop GameModeType. Mode={mode}", type.Name);

            if (type == GameModeType.Pause)
            {
                var multiplayerParticipant = GetMultiplayerParticipant();
                multiplayerParticipant.Unpause();
            }

            return true;
        }

        public bool CanLeaveArea()
        {
            return !_multiplayerClient.IsActive;
        }

        /// <summary>
        /// host - store roll
        /// client - ignore
        /// </summary>
        /// <param name="ruleRollDice"></param>
        public void OnAfterRuleRollDiceTrigger(RuleRollDice ruleRollDice)
        {
            if (!_multiplayerHost.IsActive)
            {
                return;
            }

            var initiator = ruleRollDice.Initiator?.UniqueId;
            var dice = ruleRollDice.DiceFormula.Dice;
            var resultOverride = ruleRollDice.ResultOverride;
            var result = ruleRollDice.Result;
            var ruleType = ruleRollDice.Reason.Rule.GetType().Name;
            var ruleName = ruleRollDice.Reason.Name;

            switch (ruleRollDice.Reason.Rule)
            {
                case RulePartyStatCheck rulePartyStatCheck:
                    var rollUniqueId = Convert.ToBase64String(Encoding.UTF8.GetBytes(initiator + dice + ruleType + ruleName + rulePartyStatCheck.DifficultyClass + rulePartyStatCheck.StatType));
                    Main.GetLogger<PubSubPatches>().LogWarning("RuleRollDice_OnTrigger_Postfix. Initiator={initiator}, Dice={dice}, ResultOverride={resultOverride}, Result={result}, RuleName={ruleName} RuleType={ruleType}, RuleUniqueId={rollUniqueId}", initiator, dice, resultOverride, result, ruleName, ruleType, rollUniqueId);
                    break;
                case RuleRollD20:
                default:
                    Main.GetLogger<PubSubPatches>().LogWarning("RuleRollDice_OnTrigger_Postfix - Skipping dice roll. Type={rollType}", ruleRollDice.Reason.Rule.GetType().Name);
                    break;
            }
        }

        /// <summary>
        /// host - ignore
        /// client - get roll from host
        /// </summary>
        /// <param name="ruleRollDice"></param>
        /// <returns></returns>
        public bool OnBeforeRuleRollDiceTrigger(RuleRollDice ruleRollDice)
        {
            if (!_multiplayerClient.IsActive)
            {
                return true;
            }

            var initiator = ruleRollDice.Initiator?.UniqueId;
            var dice = ruleRollDice.DiceFormula.Dice;
            var resultOverride = ruleRollDice.ResultOverride;
            var ruleType = ruleRollDice.Reason.Rule.GetType().Name;
            var ruleName = ruleRollDice.Reason.Name;

            switch (ruleRollDice.Reason.Rule)
            {
                case RulePartyStatCheck rulePartyStatCheck:
                    var rollUniqueId = Convert.ToBase64String(Encoding.UTF8.GetBytes(initiator + dice + ruleType + ruleName + rulePartyStatCheck.DifficultyClass + rulePartyStatCheck.StatType));
                    if (rollUniqueId == "YTk1MGFkNzUtNjVjZC00ZGMxLTk2ZTktNDQ0ZTI5MWZlZDdlRDIwUnVsZVBhcnR5U3RhdENoZWNrMTJDaGVja0RpcGxvbWFjeQ==")
                    {
                        Thread.Sleep(200); // network latency
                        Main.GetLogger<PubSubPatches>().LogWarning("RuleRollDice_OnTrigger_Prefix - Using pregenerated values for RulePartyStatCheck");
                        ruleRollDice.m_Result = 36;
                        return false;
                    }
                    break;
                case RuleRollD20:
                default:
                    break;
            }

            return true;
        }

        private IMultiplayerParticipant GetMultiplayerParticipant()
        {
            return _multiplayerHost.IsActive ? _multiplayerHost : _multiplayerClient;
        }

        private void ShowEscMenuMultiplayerLobby()
        {
            _logger.LogInformation("Show lobby window");
            _lobbyWindow.Show(true);
        }

        private void ShowMultiplayerWindow()
        {
            _logger.LogInformation("Show Multiplayer window");
            _multiplayerWindow.Show(true);
        }

        private NetworkGame GetNetworkGame()
        {
            return _multiplayerHost.IsActive ? _multiplayerHost.CurrentGame : _multiplayerClient.CurrentGame;
        }

        private void OnLobbyCharacterOwnerChanged(int characterIndex, int playerIndex)
        {
            _logger.LogInformation("OnLobbyCharacterOwnerChanged. CharacterIndex={charIndex}, PlayerIndex={playerIndex}", characterIndex, playerIndex);
            _multiplayerHost.ChangeCharacterOwner(characterIndex, playerIndex);
        }
    }
}
