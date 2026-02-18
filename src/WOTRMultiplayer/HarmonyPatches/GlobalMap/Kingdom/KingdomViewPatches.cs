using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Kingdom;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap.Kingdom
{
    public class KingdomViewPatches
    {
        [HarmonyPatch(typeof(KingdomPCView), nameof(KingdomPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void KingdomPCView_BindViewImplementation_Postfix(KingdomPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            //Main.Multiplayer.OnKingdomLoaded();
        }
    }
}
