using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkCombat
    {
        public bool IsInitialized { get; set; }

        public int Round { get; set; }

        public NetworkCombatTurn Turn { get; set; }

        public ConcurrentDictionary<long, bool> PlayersCombatInitialization { get; set; } = new();

        /// <summary>
        /// key: round+unitid
        /// </summary>
        public ConcurrentDictionary<string, HashSet<long>> PlayersTurnStartInitialization { get; set; } = new();

        /// <summary>
        /// key: round+unitid
        /// </summary>
        public ConcurrentDictionary<string, HashSet<long>> PlayersTurnEndInitialization { get; set; } = new();
    }
}
