using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.Hashing;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTRMultiplayer.MP
{
    public class DiceRollStorage : IDiceRollStorage
    {
        private readonly ConcurrentDictionary<int, DiceRollEntry> _rolls = new();
        private readonly ILogger<DiceRollStorage> _logger;
        private readonly IHashService _hashService;
        private readonly TimeSpan _defaultRetrieveDelay = TimeSpan.FromMilliseconds(50);

        public DiceRollStorage(
            ILogger<DiceRollStorage> logger,
            IHashService hashService)
        {
            _logger = logger;
            _hashService = hashService;
        }

        public bool Save(NetworkDiceRoll diceRoll)
        {
            try
            {
                var rollId = GetUniqueId(diceRoll);
                var entry = new DiceRollEntry(diceRoll);
                if (!_rolls.TryAdd(rollId, entry))
                {
                    _logger.LogError("Collision has been detected. Type={type}, RollId={rollId}", diceRoll.GetType().Name, rollId);
                    return false;
                }

                _logger.LogInformation("Roll has been preserved. Type={type}, UniqueId={rollId}, Result={result}, TotalBonus={totalBonus}, Initiator={initiator}, HistoryCount={historyCount}", diceRoll.GetType().Name, rollId, diceRoll.Result, diceRoll.TotalModifiersBonus, diceRoll.InitiatorId, diceRoll.RollHistory.Count);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unable to add roll");
                throw;
            }
        }

        public NetworkDiceRoll Get(int rollId, long playerId, bool ensureCompleted = true)
        {
            try
            {
                if (!_rolls.TryGetValue(rollId, out var entry))
                {
                    return null;
                }

                if (ensureCompleted && !entry.Roll.IsCompleted())
                {
                    return null;
                }

                entry.RetrieveHistory.AddOrUpdate(playerId, k => 1, (key, existing) =>
                {
                    var accessCount = existing + 1;
                    _logger.LogWarning("Same roll has been retrieved more than once by the same player. PlayerId={playerId}, RollId={rollId}, AccessCount={accessCount}", playerId, rollId, accessCount);
                    return accessCount;
                });

                return entry.Roll;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable retrieve roll. RollId={rollId}, PlayerId={playerId}", rollId, playerId);
                throw;
            }
        }

        public async Task<NetworkDiceRoll> GetAsync(int rollId, long playerId, TimeSpan? waitForRollTimeout)
        {
            var timeoutTask = waitForRollTimeout == null ? Task.CompletedTask : Task.Delay(waitForRollTimeout.Value);
            NetworkDiceRoll result;
            do
            {
                result = Get(rollId, playerId, ensureCompleted: true);
                if (result == null)
                {
                    await Task.Delay(_defaultRetrieveDelay);
                }
            }
            while (result == null && !timeoutTask.IsCompleted);

            return result;
        }

        public int GetUniqueId(NetworkDiceRoll roll)
        {
            var rawRollId = roll.GetIdString();
            var rollId = _hashService.Murmur3(rawRollId);
            return rollId;
        }

        public void Reset()
        {
            _rolls.Clear();
            _logger.LogInformation("All rolls have been cleared");
        }

        public void Reset<T>()
            where T : NetworkDiceRoll
        {
            var rollsToRemove = _rolls.Where(x => x.Value.Roll is T).ToList();
            foreach (var roll in rollsToRemove)
            {
                _rolls.TryRemove(roll.Key, out _);
            }

            _logger.LogInformation("Typed rolls have been cleared. RollType={rollType}, RemovedCount={removedCount}", typeof(T), rollsToRemove.Count);
        }
    }
}
