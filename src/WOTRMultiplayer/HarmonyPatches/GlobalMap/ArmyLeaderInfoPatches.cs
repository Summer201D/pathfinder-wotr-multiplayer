using System;
using HarmonyLib;
using Kingmaker.UI.MVVM._VM.Crusade.ArmyInfo;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class ArmyLeaderInfoPatches
    {
        [HarmonyPatch(typeof(ArmyLeaderInfoVM), nameof(ArmyLeaderInfoVM.OnClick))]
        [HarmonyPrefix]
        public static void ArmyLeaderInfoVM_OnClick_Prefix(ArmyLeaderInfoVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            OnArmyInfoArmyLeaderAction(__instance,
            () =>
            {
                Main.Multiplayer.OnGlobalMapCrusadeArmyMainLeaderAction();
            },
            () =>
            {
                Main.Multiplayer.OnGlobalMapCrusadeArmyMergeLeaderAction();
            });
        }

        [HarmonyPatch(typeof(ArmyLeaderInfoVM), nameof(ArmyLeaderInfoVM.OnLevelUp))]
        [HarmonyPrefix]
        public static void ArmyLeaderInfoVM_OnLevelUp_Prefix(ArmyLeaderInfoVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            OnArmyInfoArmyLeaderAction(__instance,
            () =>
            {
                Main.Multiplayer.OnGlobalMapCrusadeArmyMainLeaderLevelUp();
            },
            () =>
            {
                Main.Multiplayer.OnGlobalMapCrusadeArmyMergeLeaderLevelUp();
            });
        }

        [HarmonyPatch(typeof(ArmyLeaderInfoVM), nameof(ArmyLeaderInfoVM.OnLookAtLeaderPool))]
        [HarmonyPrefix]
        public static void ArmyLeaderInfoVM_OnLookAtLeaderPool_Prefix(ArmyLeaderInfoVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            OnArmyInfoArmyLeaderAction(__instance,
            () =>
            {
                Main.Multiplayer.OnGlobalMapCrusadeArmyMainLeaderLookAtPool();
            },
            () =>
            {
                Main.Multiplayer.OnGlobalMapCrusadeArmyMergeLeaderLookAtPool();
            });
        }

        private static void OnArmyInfoArmyLeaderAction(ArmyLeaderInfoVM __instance, Action onMainCart, Action onMergeCart)
        {
            var armyInfo = Main.UIAccessor.GlobalMapPCView?.m_ArmyInfoPCView;
            if (armyInfo?.m_MainArmyCartView?.m_LeaderInfoView?.ViewModel == __instance)
            {
                onMainCart?.Invoke();
            }
            else if (armyInfo?.m_MergeArmyCartView?.m_LeaderInfoView?.ViewModel == __instance)
            {
                onMergeCart?.Invoke();
            }

        }
    }
}
