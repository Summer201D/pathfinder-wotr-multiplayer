using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkGame
    {
        public List<string> Portraits { get; set; } = [];

        public NetworkGameStatus Status { get; set; }

        public List<NetworkPlayer> Players { get; set; } = [];

        public NetworkGame()
        {
            Status = NetworkGameStatus.Lobby;
        }
    }
}
