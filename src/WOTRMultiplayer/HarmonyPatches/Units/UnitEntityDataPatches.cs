using HarmonyLib;
using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.HarmonyPatches.Units
{
    [HarmonyPatch]
    public class UnitEntityDataPatches
    {
        [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.IsDirectlyControllable), MethodType.Getter)]
        [HarmonyPostfix]
        public static void UnitEntityData_IsDirectlyControllable_Postfix(UnitEntityData __instance, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __result = Main.Multiplayer.CanControlCharacter(__result, __instance.UniqueId);
        }
    }
}
