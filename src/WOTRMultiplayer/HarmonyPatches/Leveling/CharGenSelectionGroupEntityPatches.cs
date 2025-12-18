using System;
using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases.Mythic;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Mythic;
using Owlcat.Runtime.UI.SelectionGroup;
using Owlcat.Runtime.UI.SelectionGroup.View;
using UniRx;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class CharGenSelectionGroupEntityPatches
    {
        [HarmonyPatch(typeof(SelectionGroupEntityView<CharGenMythicSelectorItemVM>), nameof(SelectionGroupEntityView<CharGenMythicSelectorItemVM>.OnClick))]
        [HarmonyPrefix]
        public static bool CharGenMythicSelectorItemPCView_OnClick_Prefix(SelectionGroupEntityView<SelectionGroupEntityVM> __instance)
        {
            if (!Main.Multiplayer.IsActive || __instance.GetType() != typeof(CharGenMythicSelectorItemPCView))
            {
                return true;
            }

            var type = __instance.GetType();
            if (type == typeof(CharGenMythicSelectorItemPCView))
            {
                var canContinue = Main.Multiplayer.CanMakeLevelingDecisions();
                return canContinue;
            }

            return true;
        }

        [HarmonyPatch(typeof(SelectionGroupEntityView<SelectionGroupEntityVM>), nameof(SelectionGroupEntityView<SelectionGroupEntityVM>.BindViewImplementation))]
        [HarmonyPostfix]
        public static void SelectionGroupEntityView_BindViewImplementation_Postfix(SelectionGroupEntityView<SelectionGroupEntityVM> __instance)
        {
            if (!Main.Multiplayer.IsActive || __instance.GetType() != typeof(CharGenMythicSelectorItemPCView))
            {
                return;
            }

            var type = __instance.GetType();
            if (type == typeof(CharGenMythicSelectorItemPCView))
            {
                __instance.AddDisposable(LevelingMythicClassSelected((CharGenMythicSelectorItemVM)__instance.ViewModel));
            }
        }

        private static IDisposable LevelingMythicClassSelected(CharGenMythicSelectorItemVM viewModel)
        {
            return viewModel.IsSelected.Subscribe<bool>(isSelected =>
            {
                if (!isSelected)
                {
                    return;
                }

                var classId = viewModel.Class.AssetGuid.ToString();
                Main.Multiplayer.OnLevelingMythicClassSelected(classId);
            });
        }
    }
}
