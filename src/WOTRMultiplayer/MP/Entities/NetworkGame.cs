using System.Collections.Generic;
using System.Net;
using Kingmaker.EntitySystem.Persistence;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkGame
    {
        public long LocalPlayerId { get; set; }

        public EndPoint Endpoint { get; set; }

        public NetworkGameStage Stage { get; set; }

        public List<NetworkPlayer> Players { get; set; } = [];

        public List<NetworkCharacterOwnership> Characters { get; set; } = [];

        public NetworkCombat Combat { get; set; }

        public NetworkDialog Dialog { get; set; }

        public SaveInfo Save { get; set; }

        public NetworkGame(SaveInfo save)
        {
            Save = save;
            Stage = NetworkGameStage.Lobby;
        }

        public void Reset()
        {
            LocalPlayerId = 0; // -1 host << 0 default << 1+ clients
            Players.Clear();
            Characters.Clear();
            Save = null;
            Endpoint = null;
            Stage = NetworkGameStage.None;
            Dialog = null;
        }
    }
}
