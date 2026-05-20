## Most impactful desync issues (as of now)
- **Opportunity attacks** - sometimes they don't trigger for everyone in the lobby.
- **The Last Sarkorians (Ulbrig DLC)** - undead swamp encounter can enter infinite combat.
- **Blackwater boss** - random unit spawns are desynced.
- **Environment effects** - trigger at different times because they are controlled by separate cutscene timers
  - Act2 Drezen Siege - Giants - completely disabled as of now
  - Blackwater traps
  - Act5 Iz - Blood Rain
  - Act5 The Enigma (Nenio final quest) - Exhaust statues / Undead respawner
- **Triggered traps** - AoE spells may affect different characters.
- **Working in Tandem (and similar effects)** - the attack roll bonus depends on who attacks first (mount or rider), but since this is frame-dependent, the attack order may vary.
- **Spell DC inconsistencies** (e.g., dispel checks or saving throws) - DC occasionally differs for unclear reasons (maybe difficulty DC bonus is not applied sometimes).
  - **Act 4 - Chivarro buffs** (final encounter)
  - **Act 2 – Seelah camp** - quasit poison
- **Pit spells** - uses a separate unsynced trigger timer

Most of the issues above are mitigated by syncing HP / state / auto-killing units in combat, but some would require a few save game loads

## How to deal with desync
**Option #1** - There is a hotkey to reset combat state. Basically it restarts combat with a fresh state, sometimes this comes handy

**Option #2** - kill all enemies via Toybox

**Option #3** - quick save / quick load - everyone in the lobby will load the same save

**Option #4** - complete broken segment in single player and rehost game
