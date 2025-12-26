using System.Collections.Generic;
using WOTRMultiplayer.MP.Entities.NewGame;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkGameStartUp
    {
        public bool IsNewGameSequence { get; set; }

        public List<NetworkCharacter> Characters { get; set; } = [];

        public NetworkNewGameSequencePhaseType PhaseType { get; set; }

        public HashSet<long> PlayerReadiness { get; set; } = [];

        public string SavePath { get; set; }

        public NetworkGameStartUp(string savePath)
        {
            SavePath = savePath;
            IsNewGameSequence = string.IsNullOrEmpty(savePath);
        }
    }
}
