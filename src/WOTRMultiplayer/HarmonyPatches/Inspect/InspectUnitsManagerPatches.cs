using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Inspect;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Inspect
{
    [HarmonyPatch]
    public class InspectUnitsManagerPatches
    {
        [HarmonyPatch(typeof(InspectUnitsManager), nameof(InspectUnitsManager.TryMakeKnowledgeCheck), [typeof(UnitEntityData)])]
        [HarmonyPrefix]
        public static bool InspectUnitsManager_TryMakeKnowledgeCheck_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            Main.GetLogger<InspectUnitsManagerPatches>().LogError("TODO");
            return false;
        }
    }
}
