using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.UI;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._PCView.Crusade.ArmyInfo;
using Kingmaker.UI.MVVM._VM.Crusade.ArmyInfo;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Entities.GlobalMap;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class ArmyCartSetLeaderViewPatches
    {
        [HarmonyPatch(typeof(ArmyCartSetLeaderPCView), nameof(ArmyCartSetLeaderPCView.BindViewImplementation))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ArmyCartSetLeaderPCView_BindViewImplementation_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.Method(typeof(EscHotkeyManager), nameof(EscHotkeyManager.Subscribe));
            var replaceWith = AccessTools.Method(typeof(ArmyCartSetLeaderViewPatches), nameof(ArmyCartSetLeaderViewPatches.SubscribeEnterMessageEscPress));
            var matcher = new CodeMatcher(instructions);

            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<ArmyInfoViewPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return instructions;
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match = match.RemoveInstructions(1).Insert(newInstructions);
            Main.GetLogger<ArmyInfoViewPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(ArmyCartSetLeaderPCView), nameof(ArmyCartSetLeaderPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void ArmyCartSetLeaderPCView_BindViewImplementation_Postfix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapCrusadeArmySetLeaderShown();
        }

        [HarmonyPatch(typeof(ArmyCartSetLeaderView), nameof(ArmyCartSetLeaderView.DestroyViewImplementation))]
        [HarmonyPrefix]
        public static void ArmyCartSetLeaderView_DestroyViewImplementation_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapCrusadeArmySetLeaderClosed();
        }

        [HarmonyPatch(typeof(ArmyCartSetLeaderVM), nameof(ArmyCartSetLeaderVM.OnBuyLeader))]
        [HarmonyPrefix]
        public static void ArmyCartSetLeaderVM_OnBuyLeader_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.Multiplayer.OnGlobalMapCrusadeArmyInfoSetLeaderRecruit();
        }

        [HarmonyPatch(typeof(ArmyCartSetLeaderVM), nameof(ArmyCartSetLeaderVM.OnClearLeader))]
        [HarmonyPrefix]
        public static bool ArmyCartSetLeaderVM_OnClearLeader_Prefix(ArmyCartSetLeaderVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            Main.Multiplayer.OnGlobalMapCrusadeArmySetLeaderClear();

            var popup = new NetworkGlobalMapCommonPopup { Type = NetworkGlobalMapCommonPopupType.ClearLeader };
            UIUtility.ShowMessageBox(UIStrings.Instance.CrusadeTexts.ClearCurrentLeaderRequest, MessageModalBase.ModalType.Dialog, type =>
            {
                if (type == MessageModalBase.ButtonType.Yes)
                {
                    Main.Multiplayer.OnGlobalMapCommonPopupAccepted(popup);
                    __instance.m_State.Data.Leader.DetachFromArmy();
                    __instance.UpdateLeaders();
                    return;
                }
                Main.Multiplayer.OnGlobalMapCommonPopupDeclined(popup);
            }, null, 0, null, null, null);
            Main.Multiplayer.OnGlobalMapCommonPopupShown(popup);

            return false;
        }

        private IDisposable SubscribeEnterMessageEscPress(Action action, ArmyCartSetLeaderPCView view)
        {
            return Game.Instance.UI.EscManager.Subscribe(() =>
            {
                if (!Main.Multiplayer.IsActive || view.m_CloseButton.Interactable)
                {
                    action?.Invoke();
                    return;
                }
            });
        }
    }
}
