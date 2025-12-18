using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.MVVM._CommonView.CharGen.Phases.Common;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases.AbilityScores;
using Kingmaker.UI.MVVM._PCView.GlobalMap;
using Kingmaker.UI.MVVM._PCView.InGame;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.MP.Entities.Leveling;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class SequentialSelectorCommonViewPatches
    {
        [HarmonyPatch(typeof(SequentialSelectorCommonView), nameof(SequentialSelectorCommonView.OnNextHandler))]
        [HarmonyPrefix]
        public static bool SequentialSelectorCommonView_OnNextHandler_Prefix(SequentialSelectorCommonView __instance, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var currentView = GetCurrentCharGenDetailView();
            switch (currentView)
            {
                case CharGenAbilityScoresDetailedPCView:
                    var canInteract = Main.Multiplayer.CanMakeLevelingDecisions();
                    if (!canInteract)
                    {
                        __result = false;
                        return false;
                    }

                    Main.Multiplayer.OnLevelingRacialAbilityScoreBonusChanged(NetworkLevelingSequenceDirection.Right);
                    return true;
                default:
                    return true;
            }
        }

        [HarmonyPatch(typeof(SequentialSelectorCommonView), nameof(SequentialSelectorCommonView.OnPreviousHandler))]
        [HarmonyPrefix]
        public static bool SequentialSelectorCommonView_OnPreviousHandler_Prefix(SequentialSelectorCommonView __instance, ref bool __result)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var currentView = GetCurrentCharGenDetailView();
            switch (currentView)
            {
                case CharGenAbilityScoresDetailedPCView:
                    var canInteract = Main.Multiplayer.CanMakeLevelingDecisions();
                    if (!canInteract)
                    {
                        __result = false;
                        return false;
                    }

                    Main.Multiplayer.OnLevelingRacialAbilityScoreBonusChanged(NetworkLevelingSequenceDirection.Left);
                    return true;
                default:
                    return true;
            }
        }

        private static ICharGenPhaseDetailedView GetCurrentCharGenDetailView()
        {
            var charGenContext = Game.Instance.RootUiContext.m_UIView switch
            {
                InGamePCView inGamePCView => inGamePCView.m_StaticPartPCView.m_CharGenContextPCView,
                GlobalMapPCView globalMapPCView => globalMapPCView.m_CharGenContextPCView,
                _ => null
            };

            var charGenView = charGenContext?.m_CharGenPCView;
            if (charGenView == null)
            {
                Main.GetLogger<SequentialSelectorCommonViewPatches>().LogError("Unable to find char gen pc view");
                return null;
            }

            return charGenView?.SelectedDetailView;
        }
    }
}
