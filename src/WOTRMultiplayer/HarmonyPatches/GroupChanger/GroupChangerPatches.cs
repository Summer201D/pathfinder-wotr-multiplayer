using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.GroupChanger;
using Kingmaker.UI.MVVM._VM.GroupChanger;

namespace WOTRMultiplayer.HarmonyPatches.GroupChanger
{
    [HarmonyPatch]
    public class GroupChangerPatches
    {
        [HarmonyPatch(typeof(GroupChangerPCView), nameof(GroupChangerPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void GroupManager_BindViewImplementation_Postfix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnShowGroupChangerUI();
        }

        [HarmonyPatch(typeof(GroupChangerVM), nameof(GroupChangerVM.Close))]
        [HarmonyPrefix]
        public static void GroupChangerVM_Close_Prefix(GroupChangerVM __instance)
        {
            if (!Main.Multiplayer.IsActive || !__instance.CloseCondition())
            {
                return;
            }

            Main.Multiplayer.OnCloseGroupChangerUI();
        }

        [HarmonyPatch(typeof(GroupChangerCharacterVM), nameof(GroupChangerCharacterVM.OnClick))]
        [HarmonyPrefix]
        public static bool GroupChangerCharacterVM_OnClick_Prefix(GroupChangerCharacterVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var unitId = __instance.UnitRef.UniqueId;
            var canUse = Main.Multiplayer.OnClickGroupChangerUnit(unitId);
            return canUse;
        }

        [HarmonyPatch(typeof(GroupChangerCommonVM), nameof(GroupChangerCommonVM.InternalGo))]
        [HarmonyPrefix]
        public static void GroupChangerCommonVM_InternalGo_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnAcceptGroupChangerParty();
        }
    }
}
