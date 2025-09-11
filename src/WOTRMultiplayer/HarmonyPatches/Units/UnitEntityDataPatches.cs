using HarmonyLib;
using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.HarmonyPatches.Units
{
    [HarmonyPatch]
    public class UnitEntityDataPatches
    {
        //[HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.IsDirectlyControllable), MethodType.Getter)]
        //[HarmonyPostfix]
        //public static void UnitEntityData_IsDirectlyControllable_Postfix(UnitEntityData __instance, ref bool __result)
        //{
        //    // no need to possibly override value if this character is not controllable at all
        //    if (!Main.Multiplayer.IsActive || !__result)
        //    {
        //        return;
        //    }

        //    __result = Main.Multiplayer.IsControlledByLocalPlayer(__instance.UniqueId);
        //}
    }
}
