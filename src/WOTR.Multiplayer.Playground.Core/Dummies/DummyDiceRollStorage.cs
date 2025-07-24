using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTR.Multiplayer.Playground.Core.Dummies
{
    public class DummyDiceRollStorage : IDiceRollStorage
    {
        private readonly List<NetworkDiceRoll> _rolls;

        public DummyDiceRollStorage(List<NetworkDiceRoll> rolls)
        {
            _rolls = rolls ?? [];
        }

        public bool Save(NetworkDiceRoll rollDice)
        {
            return true;
        }

        public NetworkDiceRoll Get(int rollId, long playerId, bool ensureCompleted = true)
        {
            return _rolls.FirstOrDefault();
        }

        public int GetUniqueId(NetworkDiceRoll roll)
        {
            return -1;
        }

        public void Reset()
        {
        }

        public void Reset<T>()
            where T : NetworkDiceRoll
        {
        }

        public Task<NetworkDiceRoll> GetAsync(int rollId, long playerId, TimeSpan? timeout)
        {
            return Task.FromResult(Get(rollId, playerId));
        }
    }
}
