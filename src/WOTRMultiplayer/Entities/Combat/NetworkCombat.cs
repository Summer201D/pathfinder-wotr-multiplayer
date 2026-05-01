using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WOTRMultiplayer.Entities.AreaEffects;

namespace WOTRMultiplayer.Entities.Combat
{
    public class NetworkCombat
    {
        public int Seed { get; set; }

        public NetworkCombatStage Stage { get; set; }

        public int Round { get; set; }

        public NetworkCombatTurn Turn { get; set; }

        public ConcurrentDictionary<string, HashSet<long>> PlayersNextTurnInitialization { get; set; } = new();

        public ConcurrentDictionary<string, HashSet<long>> PlayersNextTurnSynchronization { get; set; } = new();

        public ConcurrentDictionary<string, HashSet<long>> MidCombatUnitJoins { get; set; } = new();

        public HashSet<string> ConfirmedMidCombatUnits { get; set; } = [];

        public ConcurrentDictionary<string, HashSet<string>> UntargetableUnits { get; set; } = [];

        public HashSet<NetworkAreaEffect> TriggeredAreaEffects { get; set; } = [];

        public DateTime StartedAt { get; set; }

        public HashSet<string> RemotelyKilledUnits { get; set; } = [];

        public bool IsInitiated { get; set; }

        public bool IsDataCollected { get; set; }

        public bool IsDataCompared { get; set; }

        public bool IsSynced { get; set; }

        public bool IsStarted { get; set; }

        public ConcurrentDictionary<long, bool> PlayersFinishedStartupSequence { get; set; } = [];

        public ConcurrentDictionary<long, bool> PlayersInCombat { get; set; } = [];

        public ConcurrentDictionary<long, HashSet<string>> InitialUnitsInCombat { get; set; } = [];

        public ConcurrentDictionary<long, List<string>> UnavailableUnits { get; set; } = [];
    }
}
