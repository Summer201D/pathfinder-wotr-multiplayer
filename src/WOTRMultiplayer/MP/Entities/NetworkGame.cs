using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkGame
    {
        public string Name { get; set; }

        public List<string> Portraits { get; set; } = [];

        public NetworkGameStatus Status { get; set; }

        public List<NetworkPlayer> Players { get; set; } = [];

        public NetworkGame(string name)
        {
            Name = name;
            Status = NetworkGameStatus.Lobby;
        }
    }
}
