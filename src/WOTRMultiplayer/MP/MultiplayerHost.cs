using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using Kingmaker.Utility;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Abstractions.IO;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.MP.Entities;
using WOTRMultiplayer.MP.Entities.Rolls;
using WOTRMultiplayer.Networking.Abstractions;
using WOTRMultiplayer.Networking.Messages.Game;
using WOTRMultiplayer.Networking.Messages.Lobby;

namespace WOTRMultiplayer.MP
{
    public class MultiplayerHost : MultiplayerActorBase, IMultiplayerHost
    {
        private readonly INetworkServer _networkServer;
        private readonly IFileSystemService _fileSystemService;
        private readonly IMultiplayerSettingsProvider _multiplayerSettingsProvider;
        private readonly IGameInteractionService _gameInteractionService;
        private readonly IDiceRollStorage _diceRollStorage;

        private NetworkGameStage Status => Game?.Stage ?? NetworkGameStage.None;

        private readonly object _actionlock = new();

        public Action<List<NetworkPlayer>> OnPlayersChanged { get; set; }
        public Action<EndPoint> OnConnected { get; set; }
        public Action<string> OnStartGame { get; set; }

        public bool IsActive => _networkServer.IsActive;

        public bool IsInLobby => IsActive && Status == NetworkGameStage.Lobby;

        public NetworkGame CurrentGame => Game;

        protected override bool IsHost => true;

        public MultiplayerHost(
            ILogger<MultiplayerHost> logger,
            IGameInteractionService gameInteractionService,
            IMultiplayerSettingsProvider multiplayerSettingsProvider,
            IFileSystemService fileSystemService,
            INetworkServer networkServer,
            IDiceRollStorage diceRollStorage,
            IMapper mapper) : base(logger, mapper)
        {
            _networkServer = networkServer;
            _fileSystemService = fileSystemService;
            _multiplayerSettingsProvider = multiplayerSettingsProvider;
            _gameInteractionService = gameInteractionService;
            _diceRollStorage = diceRollStorage;
        }

        public void Create(string saveFilePath, List<NetworkCharacterOwnership> characters)
        {
            if (_networkServer.IsActive)
            {
                _networkServer.Dispose();
            }

            RegisterHandlers();

            Game?.Reset();

            Game = new NetworkGame(saveFilePath)
            {
                LocalPlayerId = LocalHostPlayerId
            };

            Game.Characters.AddRange(characters);

            _networkServer.Start(_multiplayerSettingsProvider.Settings.HostPortRangeStart, _multiplayerSettingsProvider.Settings.HostPortRangeEnd);

            Logger.LogInformation("Host has been created. SavePath={savePath}, Portraits={portraits}", saveFilePath, string.Join(";", Game.Characters.Select(c => c.Portrait)));
        }

        public void UpdateSaveGame(string saveFilePath, List<NetworkCharacterOwnership> characters)
        {
            Game.SaveFilePath = saveFilePath;
            Game.Characters.Clear();
            Game.Characters.AddRange(characters);
            var host = GetHost();
            foreach (var character in characters)
            {
                character.Owner = host;
            }

            Logger.LogInformation("Notifying game characters changed. Portraits={portraits}", string.Join(";", Game.Characters.Select(c => c.Portrait)));
            var message = CreateNotifyGameCharactersChanged();
            _networkServer.SendAll(message);
        }

        public void ChangeCharacterOwner(int characterIndex, int playerIndex)
        {
            lock (_actionlock)
            {
                if (Game.Players.Count < playerIndex)
                {
                    Logger.LogError("Unable to change character owner as playerIndex is out of range. PlayersCount={playersCount}, PlayerIndex={playerIndex}", Game.Players.Count, playerIndex);
                    return;
                }

                var player = Game.Players[playerIndex];

                if (Game.Characters.Count < characterIndex)
                {
                    Logger.LogError("Unable to change character owner as characterIndex is out of range. CharacterOwnersCount={characterOwnersCount}, CharacterIndex={characterIndex}", Game.Characters.Count, characterIndex);
                    return;
                }

                var character = Game.Characters[characterIndex];
                character.Owner = player;
                Logger.LogInformation("New character owner. CharacterName={characterName}, PlayerId={playerId}, PlayerName={playerName}", character.Name, player.Id, player.Name);

                var charactersOwnerChanged = CreateNotifyCharactersOwnerChanged();
                _networkServer.SendAll(charactersOwnerChanged);
            }
        }

        public void MoveNonCombatCharacter(string unitId, NetworkVector3 destination, float delay, float orientation)
        {
            // TODO: current trigger couldn't be used in combat
            if (Game.Combat != null)
            {
                return;
            }

            Logger.LogInformation("Sending NotifyCharacterMove. UnitId={unitId}, Destination={destination}, Delay={delay}, Orientation={orientation}", unitId, destination, delay, orientation);
            var message = new NotifyCharacterMove
            {
                UnitId = unitId,
                Destination = new Networking.Messages.NetworkVector3(destination.X, destination.Y, destination.Z),
                Delay = delay,
                Orientation = orientation
            };
            _networkServer.SendAll(message);
        }

        public void Dispose()
        {
            Logger.LogInformation("Dispose");

            lock (_actionlock)
            {
                Game?.Reset();
            }

            _networkServer.Dispose();
        }

        public bool CanControlCharacter(string unitId)
        {
            if (Game == null)
            {
                return false;
            }

            var realCharacterId = _gameInteractionService.GetPetOwnerId(unitId) ?? unitId;

            var character = GetCharacterOwnership(realCharacterId);

            return character == null || character.Owner != null && character.Owner.Id == Game.LocalPlayerId;
        }

        public bool ReadyChanged()
        {
            var player = Game.Players.First(p => p.Id == Game.LocalPlayerId); // host should be always present
            var readyChanged = new PlayerReadyStatusChanged { PlayerId = player.Id, IsReady = !player.IsReady };
            OnPlayerReadyStatusChanged(player.Id, readyChanged);
            return readyChanged.IsReady;
        }

        public void Start()
        {
            Logger.LogInformation("Starting game...");
            // it should be fine to block current thread
            var content = _fileSystemService.GetFile(Game.SaveFilePath);
            if (content == null)
            {
                Logger.LogError("Unable to start a game due to missing save file. Path={savePath}", Game.SaveFilePath);
                return;
            }

            Game.Stage = NetworkGameStage.Initializing;
            var gameStageChanged = new NotifyGameStageChanged { Stage = Game.Stage.ToString() };
            _networkServer.SendAll(gameStageChanged);

            lock (_actionlock)
            {
                var saveGameMessageAssigned = new NotifySaveGameAssigned { Content = content, IsForceLoad = false };
                Logger.LogInformation("Sending save game file content to all players. Size={saveFileSize}", saveGameMessageAssigned.Content.Length);
                _networkServer.SendAll(saveGameMessageAssigned);
                Game.Stage = NetworkGameStage.WaitingForPlayersInitialization;
                Logger.LogInformation("Waiting for players to confirm delivery. GameStatus={gameStatus}", Game.Stage);
                GetHost().IsSyncedToStartGame = true;
            }

            TryStartGame();
        }

        public void GameLoaded()
        {
            Logger.LogInformation("Game loaded");

            // assumption: should be done after each area load aswell
            SoftReset();

            _gameInteractionService.Pause(true);

            var host = GetHost();
            host.IsLoading = false;

            TryUnpauseGame();
        }

        /// <summary>
        /// Reloads current party characters and tries to merge ownership
        /// </summary>
        public void PartyChanged()
        {
            Logger.LogInformation("Updating current characters & merging ownership");

            // could be synced from host, but state is the same anyway
            var partyCharacters = _gameInteractionService.GetPartyPlayers();
            if (partyCharacters.Count == 0)
            {
                return;
            }

            var oldCharacters = Game.Characters.ToList();
            Game.Characters = [.. partyCharacters];
            var defaultOwner = GetPlayer(Game.LocalPlayerId);
            foreach (var character in Game.Characters)
            {
                var existingOwnershipConfiguration = oldCharacters.FirstOrDefault(old =>
                    old.Name == character.Name || old.Name.Contains(character.Name));
                if (existingOwnershipConfiguration?.Owner != null)
                {
                    character.Owner = existingOwnershipConfiguration.Owner;
                    Logger.LogInformation("Character ownership has been preserved. CharacterId={characterId}, CharacterName={characterName}, Owner={ownerId}", character.UnitId, character.Name, character.Owner.Id);
                    continue;
                }

                character.Owner = defaultOwner;
                Logger.LogInformation("Character ownership has been assigned to default player (host). CharacterId={characterId}, CharacterName={characterName}, Owner={ownerId}", character.UnitId, character.Name, character.Owner.Id);
            }
        }

        public void Pause()
        {
            //Logger.LogInformation("Sending pausing notification");
            //var message = new NotifyGamePauseChanged { IsPaused = true };
            //_networkServer.SendAll(message);
        }

        public void Unpause()
        {
            Logger.LogInformation("Sending unpausing notification");
            var message = new NotifyGamePauseChanged { IsPaused = false };
            _networkServer.SendAll(message);
        }

        public void LeaveArea(string areaExitId)
        {
            Logger.LogInformation("Sending NotifyPartyLeaveArea. AreaExitId={areaExitId}", areaExitId);
            var message = new NotifyPartyLeaveArea { AreaExitId = areaExitId };
            _networkServer.SendAll(message);
        }

        public void OnAfterCueShow(string dialogName, string cueName, bool hasSystemAnswer)
        {
            Logger.LogInformation("Showing dialog Cue. DialogName={dialogName}, CueName={cueName}, HasSystemAnswer={hasSystemAnswer}", dialogName, cueName, hasSystemAnswer);
            if (hasSystemAnswer)
            {
                _gameInteractionService.SetDialogContinueButtonState(false);
            }

            if (Game.Dialog != null && Game.Dialog.Name != dialogName)
            {
                Logger.LogWarning("Previous dialog has not been disposed correctly. PreviousDialogName={previousDialogName}, CurrentDialogName={currentDialogName}", Game.Dialog.Name, dialogName);
                Game.Dialog = null;
            }

            Game.Dialog ??= new NetworkDialog(dialogName);
            Game.Dialog.CurrentCueName = cueName;
            AddCueWitness(cueName, Game.LocalPlayerId);

            TryEnableDialogContinueButton();
        }

        public bool OnBeforeSelectDialogAnswer(string dialogName, string cueName, string answerName, bool isExitAnswer, string manualUnitSelectionId)
        {
            Logger.LogInformation("Select Dialog Answer. DialogName={dialogName}, CueName={cueName} Answer={answer}, IsExitAnswer={isExitAnswer}, ManualUnitSelectionId={unitId}", dialogName, cueName, answerName, isExitAnswer, manualUnitSelectionId);

            var missingPlayers = GetPlayersWhoHaveNotSeenCueYet(cueName);
            if (missingPlayers.Count > 0)
            {
                Logger.LogWarning("Some players haven't seen the dialog yet. Players={playerNames}", string.Join(";", missingPlayers.Select(p => p.Name)));
                return false;
            }

            Game.Dialog.Answer = new NetworkDialogAnswer
            {
                AnswerName = answerName,
                CueName = cueName,
                ManualUnitSelectionId = manualUnitSelectionId
            };

            /// game will do it's 'dialog stat check' rolls logic a bit later on
            /// so answer couldn't be sent right away unless it's the last one
            if (isExitAnswer)
            {
                SendSelectedAnswer();
            }

            // resets all suggested cue answers
            _gameInteractionService.MarkSuggestedDialogAnswers([]);
            Game.Dialog.AnswerSuggestions.Clear();

            return true;
        }

        public void SendSelectedAnswer()
        {
            if (Game.Dialog == null)
            {
                Logger.LogError("Unable to send dialog answer because dialog is null");
                return;
            }

            if (Game.Dialog.Answer == null)
            {
                Logger.LogWarning("Answer is not set, most likely it's a first dialog cue or cutscene intermission. DialogName={dialogName}", Game.Dialog.Name);
                return;
            }

            Logger.LogInformation("Sending selected answer to clients. DialogName={dialogName}, CueName={cueName}, AnswerName={answerName}, ManualUnitSelectionId={unitId}", Game.Dialog.Name, Game.Dialog.Answer.CueName, Game.Dialog.Answer.AnswerName, Game.Dialog.Answer.ManualUnitSelectionId);

            var message = new NotifyDialogCueAnswerSelected
            {
                DialogName = Game.Dialog.Name,
                CueName = Game.Dialog.Answer.CueName,
                AnswerName = Game.Dialog.Answer.AnswerName,
                ManualUnitSelectionId = Game.Dialog.Answer.ManualUnitSelectionId
            };

            _networkServer.SendAll(message);
            Game.Dialog.Answer = null;
        }

        public bool StartDialog(string dialogName, string targetUnitId, string initiatorUnitId, string mapObjectId, string speakerKey)
        {
            var message = new NotifyDialogStarted
            {
                DialogName = dialogName,
                InitiatorUnitId = initiatorUnitId,
                TargetUnitId = targetUnitId,
                MapObjectId = mapObjectId,
                SpeakerKey = speakerKey
            };
            Logger.LogInformation("Sending dialog started to all clients. DialogName={dialogName}, TargetUnitId={targetUnitId}, InitiatorUnitId={initiatorUnitId}, MapObjectId={mapObjectId}, SpeakerKey={speakerKey}",
                message.DialogName, message.TargetUnitId, message.InitiatorUnitId, message.MapObjectId, message.SpeakerKey);

            _networkServer.SendAll(message);
            return true;
        }

        public void CombatStarted()
        {
            Logger.LogInformation("Combat started");
            if (Game.Combat != null)
            {
                Logger.LogWarning("Previous combat has not been disposed correctly");
            }

            Game.Combat = new NetworkCombat();

            // it's impossible to differentiate rolls between multiple combats
            _diceRollStorage.Reset<InitiativeRoll>();
            _diceRollStorage.Reset<AttackWithWeaponRoll>();
        }

        public void CombatEnded()
        {
            Logger.LogInformation("Combat ended");
            if (Game.Combat == null)
            {
                Logger.LogWarning("Combat has not been started correctly");
            }

            Game.Combat = null;
        }

        public bool CanInitializeCombat()
        {
            // host is never blocked as combat initialization (initiative rolls) are required for a clients to proceed
            return true;
        }

        public bool CanContinueCombat()
        {
            if (Game.Combat == null)
            {
                return true;
            }

            if (Game.Combat.Round == 1 && !Game.Combat.IsInitialized)
            {
                var unitsInCombat = _gameInteractionService.GetUnitsInCombat();
                var message = new NotifyCombatStarted
                {
                    Units = [.. unitsInCombat.Select(x => new Networking.Messages.NetworkUnit
                    {
                        Id = x.Id,
                        PositionX = x.Position.X,
                        PositionY = x.Position.Y,
                        PositionZ = x.Position.Z
                    })]
                };
                _networkServer.SendAll(message);
                Game.Combat.IsInitialized = true;
                Game.Combat.PlayersCombatInitialization.TryAdd(Game.LocalPlayerId, true);
                Logger.LogInformation($"Sending {nameof(NotifyCombatStarted)}. UnitsInCombat={{unitsCount}}", message.Units.Count);
            }

            return Game.Combat.PlayersCombatInitialization.Count >= Game.Players.Count;
        }

        public bool OnBeforeStartTurn(string unitId, bool actingInSurpriseRound)
        {
            try
            {
                if (Game.Combat.Turn != null && Game.Combat.Turn.IsInProgress)
                {
                    Logger.LogInformation("Turn start is allowed. UnitId={unitId}", unitId);
                    return true;
                }

                Game.Combat.Turn = new NetworkCombatTurn
                {
                    UnitId = unitId,
                    IsInProgress = false,
                    IsActingInSurpriseRound = actingInSurpriseRound,
                    IsLocalPlayer = CanControlCharacter(unitId),
                    IsAI = _gameInteractionService.IsUnitAI(unitId)
                };

                Logger.LogInformation("OnBeforeStartTurn. UnitId={unitId}, IsLocalPlayer={isLocalPlayer}, IsAI={isAI}, IsActingInSurpriseRound={isActingInSurpriseRound}",
                    unitId, Game.Combat.Turn.IsLocalPlayer, Game.Combat.Turn.IsAI, Game.Combat.Turn.IsActingInSurpriseRound);

                AddCombatTurnStartInitialization(Game.LocalPlayerId, Game.Combat.Round, unitId);

                TryStartCombatTurn();

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unable to process {nameof(OnBeforeStartTurn)}. UnitId={{unitId}}, ActingInSurpriseRound={{actingInSurpriseRound}}", unitId, actingInSurpriseRound);
                throw;
            }
        }

        public bool OnBeforeEndTurn(string unitId)
        {
            try
            {
                if (Game.Combat.Turn == null)
                {
                    Logger.LogInformation("Turn end is allowed. UnitId={unitId}", unitId);
                    return true;
                }

                // game calls this hook constantly even if you skip original (FYI: but this is not the case for OnBeforeStartTurn)
                // but we need to setup everything only once
                if (!Game.Combat.Turn.IsInProgress)
                {
                    return false;
                }

                Logger.LogInformation("OnBeforeEndTurn. UnitId={unitId}, IsLocalPlayer={isLocalPlayer}, IsAI={isAI}, IsActingInSurpriseRound={isActingInSurpriseRound}, IsInProgress={isInProgress}",
                             unitId, Game.Combat.Turn.IsLocalPlayer, Game.Combat.Turn.IsAI, Game.Combat.Turn.IsActingInSurpriseRound, Game.Combat.Turn.IsInProgress);

                AddCombatTurnEndInitialization(Game.LocalPlayerId, Game.Combat.Round, unitId);
                Game.Combat.Turn.IsInProgress = false;

                if (!Game.Combat.Turn.IsAI && Game.Combat.Turn.IsLocalPlayer)
                {
                    Logger.LogInformation("Sending turn ended to other clients. UnitId={unitId}", unitId);
                    var message = new CombatTurnEnded { Round = Game.Combat.Round, UnitId = unitId };
                    _networkServer.SendAll(message);
                }

                TryEndCombatTurn();

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unable to process {nameof(OnBeforeEndTurn)}. UnitId={{unitId}},", unitId);
                throw;
            }
        }

        public void CombatRoundStarted(int round)
        {
            Logger.LogInformation("Combat round started. Round={round}", round);
            if (Game.Combat == null)
            {
                Logger.LogWarning("Combat has not started yet");
                return;
            }

            Game.Combat.Round = round;
        }

        public int GetCombatRound()
        {
            return Game.Combat?.Round ?? 0;
        }

        public void ForceLoadGame(string savePath)
        {
            Logger.LogInformation("Notifying clients to force load save game. Path={savePath}", savePath);

            var message = new NotifySaveGameAssigned
            {
                Content = _fileSystemService.GetFile(savePath),
                IsForceLoad = true
            };

            foreach (var player in Game.Players)
            {
                player.IsLoading = true;
            }

            _networkServer.SendAll(message);
        }

        public bool ShouldStoreRoll(bool isBeforeRolling)
        {
            return IsRolledByHost(isBeforeRolling) || IsRolledByLocalPlayer(isBeforeRolling);
        }

        public NetworkDiceRoll RetrieveRoll(int networkDiceRollId, string unitId)
        {
            Logger.LogInformation("Retrieving roll from other player. RollId={rollId}, UnitId={unitId}", networkDiceRollId, unitId);

            var realCharacterId = _gameInteractionService.GetPetOwnerId(unitId) ?? unitId;
            var character = GetCharacterOwnership(realCharacterId);
            if (character == null)
            {
                Logger.LogError("Unable to find character. UnitId={unitId}", realCharacterId);
                return null;
            }

            var waitForRollTimeout = TimeSpan.FromSeconds(10);
            var message = new RollRequest { RollId = networkDiceRollId, Timeout = waitForRollTimeout };
            var playerId = character.Owner.Id;
            var response = _networkServer.SendAndWaitFor<RollResponse>(playerId, message);
            if (response == null)
            {
                Logger.LogError("Unable to retrieve roll from player. PlayerId={playerId}, RollId={rollId}", playerId, networkDiceRollId);
                return null;
            }

            if (response.Roll == null)
            {
                Logger.LogError("Player returned null roll. PlayerId={playerId}, RollId={rollId}", playerId, networkDiceRollId);
                return null;
            }

            var diceRoll = Mapper.Map<NetworkDiceRoll>(response.Roll);
            return diceRoll;
        }

        public void OnClickUnit(NetworkClick click)
        {
            if (!(Game.Combat?.Turn?.IsLocalPlayer ?? false) || _gameInteractionService.CombatTurnHasBeenFinished())
            {
                return;
            }

            Logger.LogInformation("Sending unit click. TargetUnitId={targetUnitId}, VectorPathCount={pathCount}", click.TargetUnitId, click.VectorPath.Count);

            var message = new NotifyUnitClicked
            {
                Click = Mapper.Map<Networking.Messages.NetworkClick>(click)
            };

            _networkServer.SendAll(message);
        }

        public void OnClickGround(NetworkClick click)
        {
            if (!(Game.Combat?.Turn?.IsLocalPlayer ?? false) || _gameInteractionService.CombatTurnHasBeenFinished())
            {
                return;
            }

            Logger.LogInformation("Sending ground click. WorldPosition={worldPosition}, VectorPathCount={pathCount}, SelectedUnits={selectedUnits}", click.WorldPosition, click.VectorPath.Count, string.Join(";", click.SelectedUnits));
            var message = new NotifyGroundClicked
            {
                Click = Mapper.Map<Networking.Messages.NetworkClick>(click)
            };

            _networkServer.SendAll(message);
        }

        public void OnClickWithSelectedAbility(NetworkClick click)
        {
            if (!(Game.Combat?.Turn?.IsLocalPlayer ?? false) || _gameInteractionService.CombatTurnHasBeenFinished())
            {
                return;
            }

            Logger.LogInformation("Sending ability click. TargetUnitId={targetUnitId}, AbilityId={abilityId}, WorldPosition={worldPosition}, VectorPathCount={pathCount}",
                click.TargetUnitId, click.Ability.Id, click.WorldPosition, click.VectorPath.Count);

            var message = new NotifyAbilityClicked
            {
                Click = Mapper.Map<Networking.Messages.NetworkClick>(click)
            };

            _networkServer.SendAll(message);
        }

        private void TryStartCombatTurn()
        {
            if (Game.Combat.Turn == null)
            {
                // could only happen when client starts turn before the host
                Logger.LogWarning("Trying to start turn, but it has not been initialized yet. Round={round}", Game.Combat.Round);
                return;
            }

            if (Game.Combat.Turn.IsInProgress)
            {
                Logger.LogWarning("Turn is already in progress. Round={round}, UnitId={unitId}", Game.Combat.Round, Game.Combat.Turn.UnitId);
                return;
            }

            List<NetworkPlayer> notReadyPlayers = [.. Game.Players];
            lock (_actionlock)
            {
                var key = GetTurnInitializationKey(Game.Combat.Round, Game.Combat.Turn.UnitId);
                if (Game.Combat.PlayersTurnStartInitialization.TryGetValue(key, out var readyToStartPlayers))
                {
                    notReadyPlayers.RemoveAll(p => readyToStartPlayers.Contains(p.Id));
                }

                if (notReadyPlayers.Count == 0)
                {
                    var message = new NotifyCombatTurnStarted { Round = Game.Combat.Round, UnitId = Game.Combat.Turn.UnitId };
                    _networkServer.SendAll(message);

                    Game.Combat.Turn.IsInProgress = true;
                    _gameInteractionService.StartTurnBasedCombatTurn(Game.Combat.Turn.IsActingInSurpriseRound);
                    return;
                }
            }

            Logger.LogInformation("Turn can't be started yet. Round={round}, UnitId={unitId}, NotReadyPlayers={notReadyPlayers}", Game.Combat.Round, Game.Combat.Turn.UnitId, string.Join(";", notReadyPlayers.Select(p => p.Name)));
        }

        private void AddCombatTurnStartInitialization(long playerId, int round, string unitId)
        {
            try
            {
                lock (_actionlock)
                {
                    var key = GetTurnInitializationKey(round, unitId);
                    Game.Combat.PlayersTurnStartInitialization.AddOrUpdate(key,
                        key => new HashSet<long>(collection: [playerId]),
                        (key, existing) =>
                        {
                            existing.Add(playerId);
                            return existing;
                        });

                    Logger.LogInformation("TurnStart initialization has been added. Key={key}, PlayersCount={playersCount}, KeysCount={keysCount}", key, Game.Combat.PlayersTurnStartInitialization[key].Count, Game.Combat.PlayersTurnStartInitialization.Keys.Count);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to add TurnStart initialization. PlayerId={playerId}, Round={round}, UnitId={unitId}", playerId, round, unitId);
                throw;
            }
        }

        private void AddCombatTurnEndInitialization(long playerId, int round, string unitId)
        {
            try
            {
                lock (_actionlock)
                {
                    var key = GetTurnInitializationKey(round, unitId);
                    Game.Combat.PlayersTurnEndInitialization.AddOrUpdate(key,
                        key => new HashSet<long>(collection: [playerId]),
                        (key, existing) =>
                        {
                            existing.Add(playerId);
                            return existing;
                        });
                    Logger.LogInformation("TurnEnd initialization has been added. Key={key}, PlayersCount={playersCount}, KeysCount={keysCount}", key, Game.Combat.PlayersTurnEndInitialization[key].Count, Game.Combat.PlayersTurnStartInitialization.Keys.Count);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to add TurnEnd initialization. PlayerId={playerId}, Round={round}, UnitId={unitId}", playerId, round, unitId);
                throw;
            }
        }

        private string GetTurnInitializationKey(int round, string unitId)
        {
            return $"{round}-{unitId}";
        }

        private void TryEndCombatTurn()
        {
            if (Game.Combat.Turn == null)
            {
                // could only happen when client starts turn before the host
                Logger.LogWarning("Trying to end already ended turn. Round={round}", Game.Combat.Round);
                return;
            }

            List<NetworkPlayer> notReadyPlayers = [.. Game.Players];
            lock (_actionlock)
            {
                var key = GetTurnInitializationKey(Game.Combat.Round, Game.Combat.Turn.UnitId);
                if (Game.Combat.PlayersTurnEndInitialization.TryGetValue(key, out var readyToEndPlayers))
                {
                    notReadyPlayers.RemoveAll(p => readyToEndPlayers.Contains(p.Id));
                }

                if (notReadyPlayers.Count == 0)
                {
                    var message = new NotifyCombatTurnEnded { Round = Game.Combat.Round, UnitId = Game.Combat.Turn.UnitId };
                    _networkServer.SendAll(message);

                    if (Game.Combat.Turn != null)
                    {
                        Game.Combat.Turn = null;
                        _gameInteractionService.EndTurnBasedCombatTurn();
                    }
                    return;
                }
            }

            Logger.LogInformation("Turn can't be ended yet. NotReadyPlayers={notReadyPlayers}", string.Join(";", notReadyPlayers.Select(p => p.Name)));
        }

        private NetworkCharacterOwnership GetCharacterOwnership(string unitId)
        {
            return Game.Characters.FirstOrDefault(c => string.Equals(c.UnitId, unitId, StringComparison.OrdinalIgnoreCase));
        }

        private void SoftReset()
        {
            Logger.LogInformation("Doing soft reset");
            Game.Dialog = null;
            Game.SaveFilePath = null;
            Game.Combat = null;
            _diceRollStorage.Reset();
        }

        private void AddCueWitness(string cueName, long playerId)
        {
            if (Game.Dialog == null)
            {
                Logger.LogError("Trying to add witness to null dialog. CueName={cueName}, PlayerId={playerId}", cueName, playerId);
                return;
            }

            Game.Dialog.CueViews.AddOrUpdate(cueName, (key) => new HashSet<long>([playerId]), (key, existing) =>
            {
                existing.Add(playerId);
                return existing;
            });

            Logger.LogInformation("Cue witness has been added. CueName={cueName}, PlayerId={playerId}", cueName, playerId);
        }

        private List<NetworkPlayer> GetPlayersWhoHaveNotSeenCueYet(string cueName)
        {
            if (Game.Dialog == null)
            {
                Logger.LogWarning("Trying to get cue players, but dialog is null. CueName={cueName}", cueName);
                return [];
            }

            if (!Game.Dialog.CueViews.TryGetValue(cueName, out var cueViews))
            {
                Logger.LogWarning("Specified cue doesn't exist in the views history. CueName={cueName}", cueName);
                return [];
            }

            var players = Game.Players.Where(p => !cueViews.Contains(p.Id)).ToList();
            return players;
        }

        private void TryEnableDialogContinueButton()
        {
            if (Game.Dialog == null)
            {
                Logger.LogError("Unable to enable continue button because current dialog is null");
                return;
            }

            var currentCue = Game.Dialog.CurrentCueName;
            if (string.IsNullOrEmpty(currentCue))
            {
                Logger.LogError("Current CueName is not set for the dialog");
                return;
            }

            var missingPlayers = GetPlayersWhoHaveNotSeenCueYet(currentCue);
            if (missingPlayers.Count > 0)
            {
                Logger.LogInformation("Cannot proceed with dialog yet. CurrentCue={currentCue}, MissingPlayers={missingPlayers}", currentCue, string.Join(";", missingPlayers.Select(x => x.Name)));
                return;
            }

            Logger.LogInformation("All players have witnessed current cue. CueName={cueName}", currentCue);
            _gameInteractionService.SetDialogContinueButtonState(true);
        }

        private void TryUnpauseGame()
        {
            var canUnpause = false;

            lock (_actionlock)
            {
                canUnpause = Game.Players.All(p => !p.IsLoading);
            }

            if (canUnpause)
            {
                Logger.LogInformation("All players have finished loading. Game will be unpaused");
                Game.Stage = NetworkGameStage.Playing;
                _gameInteractionService.Pause(false);
            }
        }

        private void OnClientGameLoaded(long playerId, ClientGameLoaded loaded)
        {
            Logger.LogInformation($"Received {nameof(ClientGameLoaded)}. PlayerId={{playerId}}", playerId);
            lock (_actionlock)
            {
                var player = GetPlayer(playerId);
                if (player == null)
                {
                    Logger.LogError("Can't set loading status for missing player. PlayerId={playerId}", playerId);
                    return;
                }

                player.IsLoading = false;
            }

            TryUnpauseGame();
        }

        private void TryStartGame()
        {
            var canStart = false;

            lock (_actionlock)
            {
                canStart = Game.Players.All(p => p.IsSyncedToStartGame);
            }

            if (canStart)
            {
                Logger.LogInformation("Starting game");
                foreach (var player in Game.Players)
                {
                    player.IsLoading = true;
                }

                _networkServer.SendAll(new NotifyGameStarted());
                OnStartGame?.Invoke(Game.SaveFilePath);
            }
        }

        private NotifyCharactersOwnerChanged CreateNotifyCharactersOwnerChanged()
        {
            Game.Characters.Select((character, index) => new Networking.Messages.NetworkCharacterOwner { CharacterIndex = index, PlayerId = character.Owner.Id });
            var charactersOwnerChanged = new NotifyCharactersOwnerChanged
            {
                Owners = [.. Game.Characters.Select((character, index) => new Networking.Messages.NetworkCharacterOwner { CharacterIndex = index, PlayerId = character.Owner.Id })]
            };

            return charactersOwnerChanged;
        }

        private NetworkPlayer GetHost()
        {
            return Game.Players.First(f => f.Id == Game.LocalPlayerId);
        }

        private void RegisterHandlers()
        {
            _networkServer.OnClientConnected = OnPlayerConnected;
            _networkServer.OnClientDisconnected = OnPlayerDisconnected;
            _networkServer.OnServerStarted = OnServerStarted;

            _networkServer
                // this is special case when client sends notify as usually all notifies are sent by host only
                // we need to load game ASAP on both host/remaining clients
                .Register<NotifySaveGameAssigned>(OnNotifySaveGameAssigned)
                .Register<NotifyUnitClicked>(OnNotifyUnitClicked)
                .Register<NotifyGroundClicked>(OnNotifyGroundClicked)
                .Register<NotifyAbilityClicked>(OnNotifyAbilityClicked)

                // this is kinda special as well as the client is blocking the game loop thread until `RollResponse` is received
                .Register<RollRequest>(OnRollRequest)

                .Register<PlayerReadyStatusChanged>(OnPlayerReadyStatusChanged)
                .Register<PlayerNameResponse>(OnPlayerNameResponse)
                .Register<PlayerSaveGameSyncChanged>(OnPlayerSaveGameSyncChanged)
                .Register<CharacterMove>(OnCharacterMove)
                .Register<ClientGameLoaded>(OnClientGameLoaded)
                .Register<GamePauseChanged>(OnGamePauseChanged)
                .Register<CueWitnessed>(OnCueWitnessed)
                .Register<DialogCueAnswerSuggested>(OnDialogCueAnswerSuggested)
                .Register<StartDialogRequested>(OnStartDialogRequested)
                .Register<ClientCombatInitialized>(OnClientCombatInitialized)
                .Register<ClientCombatTurnStarted>(OnClientCombatTurnStarted)
                .Register<CombatTurnEnded>(OnCombatTurnEnded)
                ;
        }

        private void OnNotifyAbilityClicked(long playerId, NotifyAbilityClicked clicked)
        {
            Logger.LogInformation($"Received {nameof(NotifyAbilityClicked)}. AbilityId={{abilityId}}, TargetUnitId={{targetUnitId}}, SelectedUnitId={{selectedUnits}}, WorldPosition={{worldPosition}}", clicked.Click.Ability.Id, clicked.Click.TargetUnitId, clicked.Click.SelectedUnits.Count, clicked.Click.WorldPosition);
            if (Game.Combat == null)
            {
                Logger.LogWarning($"{nameof(NotifyAbilityClicked)} is ignored out of combat");
                return;
            }

            var click = Mapper.Map<NetworkClick>(clicked.Click);
            _gameInteractionService.ClickAbilityInCombat(click);

            Logger.LogInformation($"Resending {nameof(NotifyAbilityClicked)} to other players");
            _networkServer.SendAllExcept(playerId, clicked);
        }

        private void OnNotifyGroundClicked(long playerId, NotifyGroundClicked clicked)
        {
            Logger.LogInformation($"Received {nameof(NotifyGroundClicked)}. PlayerId={{playerId}}, SelectedUnitId={{selectedUnits}}, WorldPosition={{worldPosition}}", playerId, clicked.Click.SelectedUnits.Count, clicked.Click.WorldPosition);
            if (Game.Combat == null)
            {
                Logger.LogWarning($"{nameof(NotifyGroundClicked)} is ignored out of combat");
                return;
            }

            var click = Mapper.Map<NetworkClick>(clicked.Click);
            _gameInteractionService.ClickGroundInCombat(click);

            Logger.LogInformation($"Resending {nameof(NotifyGroundClicked)} to other players");
            _networkServer.SendAllExcept(playerId, click);
        }

        private void OnNotifyUnitClicked(long playerId, NotifyUnitClicked clicked)
        {
            Logger.LogInformation($"Received {nameof(NotifyUnitClicked)}. PlayerId={{playerId}}, TargetUnitId={{targetUnitId}}, SelectedUnits={{selectedUnits}}", playerId, clicked.Click.TargetUnitId, clicked.Click.SelectedUnits.Count);
            if (Game.Combat == null)
            {
                Logger.LogWarning($"{nameof(NotifyUnitClicked)} is ignored out of combat");
                return;
            }

            var click = Mapper.Map<NetworkClick>(clicked.Click);
            _gameInteractionService.ClickUnitInCombat(click);

            Logger.LogInformation($"Resending {nameof(NotifyUnitClicked)} to other players");
            _networkServer.SendAllExcept(playerId, click);
        }

        private void OnCombatTurnEnded(long playerId, CombatTurnEnded ended)
        {
            Logger.LogInformation($"Received {nameof(CombatTurnEnded)}. PlayerId={{playerId}}, Round={{round}}, UnitId={{unitId}}", playerId, ended.Round, ended.UnitId);

            AddCombatTurnEndInitialization(playerId, ended.Round, ended.UnitId);

            if (!Game.Combat.Turn.IsAI && !Game.Combat.Turn.IsLocalPlayer)
            {
                Logger.LogInformation("Current turn is owned by another player. Ending it locally.  PlayerId={playerId}, Round={round}, UnitId={unitId}", playerId, ended.Round, ended.UnitId);
                OnBeforeEndTurn(ended.UnitId);
                _networkServer.SendAllExcept(playerId, ended);
                return;
            }

            TryEndCombatTurn();
        }

        private void OnClientCombatTurnStarted(long playerId, ClientCombatTurnStarted started)
        {
            Logger.LogInformation($"Received {nameof(ClientCombatTurnStarted)}. PlayerId={{playerId}}, Round={{round}}, UnitId={{unitId}}", playerId, started.Round, started.UnitId);
            AddCombatTurnStartInitialization(playerId, started.Round, started.UnitId);
            TryStartCombatTurn();
        }

        private void OnNotifySaveGameAssigned(long playerId, NotifySaveGameAssigned assigned)
        {
            Logger.LogInformation($"Received {nameof(NotifySaveGameAssigned)}. PlayerId={{playerId}}, IsForceLoad={{isForceLoad}}, SaveGameSize={{saveGameSize}}", playerId, assigned.IsForceLoad, assigned.Content.Length);

            _networkServer.SendAllExcept(playerId, assigned);

            var baseUnityPath = _gameInteractionService.GetSaveGamePath();
            var multiplayerPath = Regex.Replace(baseUnityPath, "(((\\\\|\\/)+)(Saved Games)((\\\\|\\/)+))$", "/Saved Multiplayer Games/");
            var savePath = Path.Combine(multiplayerPath, "latest save.zks");
            Logger.LogInformation("Save game path changed. Path={path}", savePath);
            if (!_fileSystemService.WriteFile(savePath, assigned.Content))
            {
                Logger.LogError("Unable to store save game");
                // on error?
                return;
            }

            Game.SaveFilePath = savePath;

            Logger.LogInformation("Game is ready to be started. SavePath={savePath}", savePath);
            if (assigned.IsForceLoad)
            {
                Logger.LogInformation("Force loading save game. SavePath={savePath}", savePath);
                _gameInteractionService.QuickLoadGame(savePath);
            }
        }

        private void OnClientCombatInitialized(long playerId, ClientCombatInitialized initialized)
        {
            Logger.LogInformation($"Received {nameof(ClientCombatInitialized)}. PlayerId={{playerId}}", playerId);
            if (Game.Combat == null)
            {
                Logger.LogWarning("Received client initialization, but combat is null. PlayerId={playerId}", playerId);
                return;
            }

            if (!Game.Combat.PlayersCombatInitialization.TryAdd(playerId, true))
            {
                Logger.LogWarning("Received duplicate client initialization. PlayerId={playerId}", playerId);
            }
        }

        private async void OnStartDialogRequested(long playerId, StartDialogRequested requested)
        {
            Logger.LogInformation($"Received {nameof(StartDialogRequested)}. PlayerId={{playerId}}, DialogName={{dialogName}}, TargetUnitId={{targetUnitId}}, InitiatorUnitId={{initiatorUnitId}}, MapObjectId={{mapObjectId}}, SpeakerKey={{speakerKey}}",
                playerId, requested.DialogName, requested.TargetUnitId, requested.InitiatorUnitId, requested.MapObjectId, requested.SpeakerKey);

            var hasStartedDialog = await _gameInteractionService.StartDialogAsync(requested.DialogName, requested.TargetUnitId, requested.InitiatorUnitId, requested.MapObjectId, requested.SpeakerKey);
            if (!hasStartedDialog)
            {
                Logger.LogInformation("Host dialog is already in progress. Sending dialog confirmation");
                var message = new NotifyDialogStarted
                {
                    DialogName = requested.DialogName,
                    InitiatorUnitId = requested.InitiatorUnitId,
                    TargetUnitId = requested.TargetUnitId,
                    MapObjectId = requested.MapObjectId,
                    SpeakerKey = requested.SpeakerKey
                };

                _networkServer.SendAll(message);
            }
        }

        private void OnDialogCueAnswerSuggested(long playerId, DialogCueAnswerSuggested suggested)
        {
            Logger.LogInformation($"Received {nameof(DialogCueAnswerSuggested)}. PlayerId={{playerId}}, DialogName={{dialogName}}, CueName={{cueName}}, AnswerName={{answerName}}", playerId, suggested.DialogName, suggested.CueName, suggested.AnswerName);

            if (Game.Dialog == null)
            {
                Logger.LogError("Received dialog answer suggestion, but there is no active dialog right now. SuggestedDialogName={suggestedDialogName}, SuggestedCueName={suggestedCueName}, SuggestedAnswer={suggestedAnswerName}", suggested.DialogName, suggested.CueName, suggested.AnswerName);
                return;
            }

            if (!string.Equals(Game.Dialog.Name, suggested.DialogName, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogError("Dialog suggestion has mismatched dialog name. SuggestedDialogName={suggestedDialogName}, CurrentDialogName={currentCueName}", suggested.DialogName, Game.Dialog.Name);
                return;
            }

            if (!string.Equals(Game.Dialog.CurrentCueName, suggested.CueName, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogError("Dialog suggestion has mismatched cue name. SuggestedCueName={suggestedCueName}, CurrentCueName={currentCueName}", suggested.CueName, Game.Dialog.CurrentCueName);
                return;
            }

            Game.Dialog.AnswerSuggestions.AddOrUpdate(playerId, suggested.AnswerName, (key, existing) =>
            {
                return suggested.AnswerName;
            });

            List<NetworkDialogAnswerSuggestion> suggestions = [.. Game.Dialog.AnswerSuggestions.GroupBy(x => x.Value, x => x.Key).Select(x => new NetworkDialogAnswerSuggestion { AnswerName = x.Key, Players = [.. x] })];
            _gameInteractionService.MarkSuggestedDialogAnswers(suggestions);

            var notifyMessage = new NotifyDialogCueAnswerSuggested
            {
                DialogName = suggested.DialogName,
                CueName = suggested.CueName,
                Suggestions = [.. suggestions.Select(x => new Networking.Messages.NetworkDialogAnswerSuggestion { AnswerName = x.AnswerName, Players = [.. x.Players] })],
            };
            _networkServer.SendAll(notifyMessage);
        }

        private void OnCueWitnessed(long playerId, CueWitnessed witnessed)
        {
            Logger.LogInformation($"Received {nameof(CueWitnessed)}. PlayerId={{playerId}}, DialogName={{dialogName}}, CueName={{cueName}}", playerId, witnessed.DialogName, witnessed.CueName);
            if (Game.Dialog == null)
            {
                Logger.LogError("Received cue witness, but there is no active dialog right now. WitnessedDialogName={witnessedDialogName}, WitnessedCueName={witnessedCueName}", witnessed.DialogName, witnessed.CueName);
                return;
            }

            if (!string.Equals(Game.Dialog.Name, witnessed.DialogName, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogError("Cue witness has mismatched dialog. WitnessedDialogName={witnessedDialogName}, CurrentDialogName={currentCueName}", witnessed.DialogName, Game.Dialog.Name);
                return;
            }

            AddCueWitness(witnessed.CueName, playerId);
            TryEnableDialogContinueButton();
        }

        private async void OnRollRequest(long playerId, RollRequest request)
        {
            Logger.LogInformation($"Received {nameof(RollRequest)}. PlayerId={{playerId}}, RollId={{rollId}}", playerId, request.RollId);
            // some events would occur at around the same time on client/host, but client MUST receive this dice roll from the host
            var roll = await _diceRollStorage.GetAsync(request.RollId, playerId, request.Timeout);

            var response = new RollResponse
            {
                Roll = Mapper.Map<Networking.Messages.NetworkDiceRoll>(roll)
            };

            Logger.LogInformation("Sending roll response. RollResult={rollResult}", roll?.Result ?? 0);
            _networkServer.Send(playerId, response);
        }

        private void OnGamePauseChanged(long playerId, GamePauseChanged pauseChanged)
        {
            Logger.LogInformation($"Received {nameof(GamePauseChanged)}. PlayerId={{playerId}}, IsPaused={{isPaused}}", playerId, pauseChanged.IsPaused);
            var message = new NotifyGamePauseChanged { IsPaused = pauseChanged.IsPaused };
            _networkServer.SendAllExcept(playerId, message);
            _gameInteractionService.Pause(pauseChanged.IsPaused);
        }

        private void OnCharacterMove(long playerId, CharacterMove move)
        {
            Logger.LogInformation($"Received {nameof(CharacterMove)}. PlayerId={{playerId}}, UnitId={{unitId}}, Destination={{destination}}", playerId, move.UnitId, move.Destination);

            var destination = new NetworkVector3(move.Destination.X, move.Destination.Y, move.Destination.Z);
            _gameInteractionService.MoveNonCombatCharacter(move.UnitId, destination, move.Delay, move.Orientation);

            var notifyMove = new NotifyCharacterMove
            {
                UnitId = move.UnitId,
                Destination = move.Destination,
                Delay = move.Delay,
                Orientation = move.Orientation
            };
            _networkServer.SendAllExcept(playerId, notifyMove);
        }

        private void OnPlayerSaveGameSyncChanged(long playerId, PlayerSaveGameSyncChanged changed)
        {
            Logger.LogInformation($"Received {nameof(PlayerSaveGameSyncChanged)}. PlayerId={{playerId}}, SyncStatus={{syncStatus}}", playerId, changed.IsSynced);
            lock (_actionlock)
            {
                var player = GetPlayer(playerId);
                if (player == null)
                {
                    Logger.LogError("Player is missing. Game won't start. Player Id={playerId}", playerId);
                    return;
                }

                player.IsSyncedToStartGame = changed.IsSynced;
            }

            TryStartGame();
        }

        private void OnPlayerReadyStatusChanged(long playerId, PlayerReadyStatusChanged readyStatusChanged)
        {
            lock (_actionlock)
            {
                var existingPlayer = GetPlayer(playerId);
                if (existingPlayer == null)
                {
                    Logger.LogWarning("Can't find existing player. PlayerId={playerId}", playerId);
                    return;
                }

                existingPlayer.IsReady = readyStatusChanged.IsReady;

                OnPlayersChanged?.Invoke(Game.Players);
                Logger.LogInformation("Sending ready status changed. PlayerId={playerId}, IsReady={isReady}", playerId, existingPlayer.IsReady);
                _networkServer.SendAll(readyStatusChanged);
            }
        }

        private void OnPlayerNameResponse(long playerId, PlayerNameResponse response)
        {
            try
            {
                Logger.LogInformation($"Received {nameof(PlayerNameResponse)}. PlayerId={{playerId}}, Name={{name}}", playerId, response?.Name);
                lock (_actionlock)
                {
                    var existingPlayer = GetPlayer(playerId);
                    if (existingPlayer == null)
                    {
                        Logger.LogWarning("Can't process player name update because player doesn't exist. PlayerId={playerId}, Name={name}", playerId, response?.Name);
                        return;
                    }

                    if (string.IsNullOrEmpty(response.Name))
                    {
                        Logger.LogWarning("Can't process player name update because player name is missing. PlayerId={playerId}, Name={name}", playerId, response?.Name);
                        return;
                    }

                    existingPlayer.Name = response.Name;

                    OnPlayersChanged?.Invoke(Game.Players);

                    var players = Game.Players.Select(x => new Networking.Messages.NetworkPlayer { Id = x.Id, Name = x.Name, IsReady = x.IsReady }).ToList();
                    var playersChanged = new NotifyPlayersChanged { Players = players };
                    Logger.LogInformation("Sending players changed to ALL players");
                    _networkServer.SendAll(playersChanged);

                    var notifyGameCharactersChanged = CreateNotifyGameCharactersChanged();
                    Logger.LogInformation("Sending GameCharactersChanged to new player. PlayerId={playerId}", playerId);
                    _networkServer.Send(playerId, notifyGameCharactersChanged);

                    Logger.LogInformation("Sending CharactersOwnerChanged to new player. PlayerId={playerId}", playerId);
                    var charactersOwnerChanged = CreateNotifyCharactersOwnerChanged();
                    _networkServer.Send(playerId, charactersOwnerChanged);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to handle player name response");
                throw;
            }
        }

        private void OnPlayerConnected(long playerId)
        {
            lock (_actionlock)
            {
                var existingPlayer = GetPlayer(playerId);
                if (existingPlayer != null)
                {
                    Logger.LogWarning("Player already exists. PlayerId={playerId}", playerId);
                    return;
                }

                var player = new NetworkPlayer(playerId);
                Game.Players.Add(player);
                Logger.LogInformation("Sending player name request. PlayerId={playerId}", playerId);
                _networkServer.Send(playerId, new PlayerNameRequest { ClientPlayerId = playerId });
            }
        }

        private void OnPlayerDisconnected(long playerId)
        {
            lock (_actionlock)
            {
                var existingPlayer = GetPlayer(playerId);
                if (existingPlayer == null)
                {
                    Logger.LogWarning("Nothing to cleanup since player doesn't exist. PlayerId={playerId}", playerId);
                    return;
                }

                Game.Players.Remove(existingPlayer);
                if (!string.IsNullOrEmpty(existingPlayer.Name))
                {
                    OnPlayersChanged?.Invoke(Game.Players);
                }

                // TODO: send updates to other clients
                Logger.LogError("Player disconnection is not synced with other players");

                if (Game.Stage == NetworkGameStage.Playing)
                {
                    _gameInteractionService.ShowModalMessage($"Player {existingPlayer.Name} has left the game");
                }
            }
        }

        private void OnServerStarted(EndPoint endpoint)
        {
            var hostPlayer = new NetworkPlayer(LocalHostPlayerId)
            {
                Name = _multiplayerSettingsProvider.Settings.PlayerName
            };

            Game.Players.Add(hostPlayer);
            Game.Endpoint = endpoint;

            foreach (var character in Game.Characters)
            {
                character.Owner = hostPlayer;
            }

            OnConnected?.Invoke(endpoint);
            OnPlayersChanged?.Invoke(Game.Players);
        }

        private NetworkPlayer GetPlayer(long playerId)
        {
            return Game.Players.FirstOrDefault(p => p.Id == playerId);
        }

        private NotifyGameCharactersChanged CreateNotifyGameCharactersChanged()
        {
            var message = new NotifyGameCharactersChanged
            {
                Characters = [.. Game.Characters.Select(c => new Networking.Messages.NetworkCharacterOwnership { Name = c.Name, Portrait = c.Portrait })]
            };
            return message;
        }
    }
}
