## Basics

All localization files (like `enGB.json`) live in the `./localization` folder. The mod always falls back to `enGB.json` if it can't find a file for your current language.

## File structure

Example:

```json
{
    "wotrmultiplayer": {
        "settings": {
            "general": {
                "playerName": {
                    "title": "test title",
                    "tooltip": "test tooltip"
                }
            }
        }
    }
}
```

The mod reads this JSON and turns it into key–value pairs based on the nesting.
With the example above, you will end up with following key/value pairs:

- `wotrmultiplayer.settings.general.playerName.title` : `test title`
- `wotrmultiplayer.settings.general.playerName.tooltip` : `test tooltip`

The key structure hints at where each key/value pair is used. For example, these keys are used to display settings in the game's `Settings` window. `Title` represents the setting name, while `Tooltip` is used to display extra information on hover. 

Partial localization is supported too. For example, you can create a `deDE.json` file with just one translated key - anything missing will automatically fall back to `enGB.json`.
