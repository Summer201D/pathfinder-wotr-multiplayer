using System.Collections.Concurrent;
using System.Collections.Generic;
using WOTRMultiplayer.Entities.NewGame;

namespace WOTRMultiplayer.Entities
{
    public class NetworkGameStartUp
    {
        public bool IsNewGameSequence { get; set; }

        public string Title { get; set; }

        public List<NetworkCharacter> Characters { get; set; } = [];

        public NetworkNewGameSequencePhaseType PhaseType { get; set; }

        public HashSet<long> ReadyPlayers { get; set; } = [];

        public string SavePath { get; set; }

        public bool AutoStart { get; set; }

        public List<byte> Content { get; set; }

        public int ExpectedChunks { get; set; }

        public ConcurrentDictionary<long, int> ConfirmedChunks { get; set; } = [];
    }
}
