## Prediction layer

At first, the idea was to synchronize only high-level actions (like `"clicked on a unit"`) to avoid dealing with the underlying game logic.

However, this approach quickly proved unreliable. The game's UI cannot be trusted - for example, hovering over an enemy might display a full-round attack, while in reality only a single attack will occur. This happens because the game's `"prediction"` layer is completely separate from the actual execution logic.

## Current approach

The implementation ultimately shifted to synchronizing unit commands instead. The only reliable way to determine how many attacks will occur is to wait until the `UnitAttack` command actually **starts**.

The downside is that a single player action can generate a large number of commands (e.g., multiple attack commands). In practice, only the first command needs to be synchronized - subsequent commands will be generated locally by other clients anyway (once attack is initiated). Sending duplicate commands will interrupt character attack/ability usage logic creating a desync.

For this reason, `UnitCommandsPatches.cs` contains numerous conditions to skip redundant commands.
