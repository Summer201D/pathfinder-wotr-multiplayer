using HarmonyLib;
using Kingmaker;
using Kingmaker.Designers.EventConditionActionSystem.Actions;

namespace WOTRMultiplayer.HarmonyPatches.MapObjects
{
    [HarmonyPatch]
    public class TrapCastSpellPatches
    {
        [HarmonyPatch(typeof(TrapCastSpell), nameof(TrapCastSpell.RunAction))]
        [HarmonyPrefix]
        public static bool TrapCastSpell_RunAction_Prefix(TrapCastSpell __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            // TODO: sync trap damage from host
            var isDrezenAshGiantTrap = __instance.Spell != null && Game.Instance.CurrentlyLoadedArea?.name == "DrezenCitadel_Level1" &&
                (string.Equals(__instance.Spell.AssetGuid.ToString(), "893fa025859e19b47a00c0577bfb2d88") || string.Equals(__instance.Spell.AssetGuid.ToString(), "1e4d0a15e1e623d4e9e68802eb90373d"));

            return !isDrezenAshGiantTrap;
        }
    }
}
