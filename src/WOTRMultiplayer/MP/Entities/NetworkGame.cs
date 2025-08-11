using System.Collections.Concurrent;
using System.Collections.Generic;
using Kingmaker.GameModes;
using WOTRMultiplayer.MP.Entities.Combat;
using WOTRMultiplayer.MP.Entities.Dialogs;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkGame
    {
        public string Id { get; set; }

        public int RestBanterSeed { get; set; }

        public long LocalPlayerId { get; set; }

        public NetworkGameConnectivity Connectivity { get; set; }

        public NetworkGameStage Stage { get; set; }

        public List<NetworkPlayer> Players { get; set; } = [];

        public List<NetworkCharacterOwnership> Characters { get; set; } = [];

        public ConcurrentDictionary<GameModeType, HashSet<long>> PlayersInGameMode { get; set; } = [];

        public HashSet<long> PlayersFinishedRest { get; set; } = [];

        public NetworkCombat Combat { get; set; }

        public NetworkDialog Dialog { get; set; }

        public string SaveFilePath { get; set; }

        public NetworkForcedPause ForcedPause { get; set; }

        public NetworkRandomEncounter RandomEncounter { get; set; }

        public NetworkGame(string saveFilePath)
        {
            SaveFilePath = saveFilePath;
            Stage = NetworkGameStage.Lobby;
        }

        public void Reset()
        {
            LocalPlayerId = 0; // -1 host << 0 default << 1+ clients
            Players.Clear();
            Characters.Clear();
            PlayersInGameMode.Clear();
            SaveFilePath = null;
            Connectivity = null;
            Stage = NetworkGameStage.None;
            Dialog = null;
            Id = null;
            Combat = null;
            ForcedPause = null;
            RandomEncounter = null;
            RestBanterSeed = 0;
        }
    }
}
