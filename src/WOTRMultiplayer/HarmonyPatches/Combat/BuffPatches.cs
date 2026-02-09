using System;
using HarmonyLib;
using Kingmaker.UnitLogic.Buffs;
using WOTRMultiplayer.Extensions;

namespace WOTRMultiplayer.HarmonyPatches.Combat
{
    [HarmonyPatch]
    public class BuffPatches
    {
        [HarmonyPatch(typeof(Buff), nameof(Buff.Postpone))]
        [HarmonyPrefix]
        public static bool Buff_Postpone_Prefix(Buff __instance, TimeSpan time)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            if (__instance.NextTickTime < TimeSpan.MaxValue)
            {
                __instance.NextTickTime = __instance.NextTickTime.SafeAdd(time);
            }

            if (__instance.EndTime < TimeSpan.MaxValue)
            {
                __instance.EndTime = __instance.EndTime.SafeAdd(time);
            }

            return false;
        }
    }
}
