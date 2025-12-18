using HarmonyLib;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Mythic;
using Microsoft.Extensions.Logging;
using Owlcat.Runtime.UI.SelectionGroup.View;
using UniRx;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class CharGenMythicPatches
    {
        [HarmonyPatch(typeof(SelectionGroupEntityView<CharGenMythicSelectorItemVM>), nameof(SelectionGroupEntityView<CharGenMythicSelectorItemVM>.OnClick))]
        [HarmonyPrefix]
        public static bool CharGenMythicSelectorItemPCView_OnClick_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanMakeLevelingDecisions();
            return canContinue;
        }

        [HarmonyPatch(typeof(SelectionGroupEntityView<CharGenMythicSelectorItemVM>), nameof(SelectionGroupEntityView<CharGenMythicSelectorItemVM>.BindViewImplementation))]
        [HarmonyPostfix]
        public static void CharGenMythicSelectorItemPCView_BindViewImplementation_Postfix(SelectionGroupEntityView<CharGenMythicSelectorItemVM> __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.AddDisposable(__instance.ViewModel.IsSelected.Subscribe<bool>(isSelected =>
            {
                if (!isSelected)
                {
                    return;
                }

                var classId = __instance.ViewModel.Class.AssetGuid.ToString();
                Main.Multiplayer.OnLevelingMythicClassSelected(classId);
            }));
        }
    }
}
