using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Persistence;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.LoadGame
{
    [HarmonyPatch]
    public class GamePatches
    {
        [HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
        [HarmonyPrefix]
        public static void Game_LoadGame_Postfix(SaveInfo saveInfo)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            if (Game.Instance.Player.MainCharacter == null)
            {
                Main.GetLogger<GamePatches>().LogInformation("Force load hook is skipped since player is not in the game");
                return;
            }

            Main.Multiplayer.ForceLoadGame(saveInfo);
        }
    }
}
