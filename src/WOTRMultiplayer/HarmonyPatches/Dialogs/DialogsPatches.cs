using System.Collections.Generic;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.UI.BookEvent;
using Kingmaker.UI.MVVM._VM.Dialog.Dialog;
using Kingmaker.View.MapObjects;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Dialogs
{
    [HarmonyPatch]
    public class DialogsPatches
    {
        [HarmonyPatch(typeof(DialogController), nameof(DialogController.StartDialog))]
        [HarmonyPrefix]
        public static bool DialogController_StartDialog_Prefix(DialogController __instance, BlueprintDialog dialog, UnitEntityData initiator, UnitEntityData unit, MapObjectView mapObject, LocalizedString customSpeakerName)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.StartDialog(dialog?.name, unit?.UniqueId, initiator?.UniqueId, mapObject?.UniqueId, customSpeakerName?.Key);
            if (!canContinue)
            {
                Game.Instance.Player.Dialog.Scheduled = null;
            }

            return canContinue;
        }

        [HarmonyPatch(typeof(DialogController), nameof(DialogController.SelectAnswer))]
        [HarmonyPrefix]
        public static bool DialogController_SelectAnswer_Prefix(DialogController __instance, BlueprintAnswer answer, UnitEntityData manualUnitSelection)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var isLastAnswer = answer.IsExit() || answer.NextCue.Cues.Count == 0;
            var canContinue = Main.Multiplayer.OnBeforeSelectDialogAnswer(__instance.Dialog.name, __instance.CurrentCue.name, answer.name, isLastAnswer, manualUnitSelection?.UniqueId);
            return canContinue;
        }

        [HarmonyPatch(typeof(DialogController), nameof(DialogController.PlayCue))]
        [HarmonyPostfix]
        public static void DialogController_PlayCue_Postfix(DialogController __instance, BlueprintCueBase cue)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            if (__instance.PlayingBookPage)
            {
                var dialogName = Game.Instance.DialogController.Dialog?.name;
                Main.Multiplayer.OnAfterCueShow(dialogName, cue.name, false);
                return;
            }

            Main.Multiplayer.OnAfterPlayDialogCue();
        }

        [HarmonyPatch(typeof(DialogVM), nameof(DialogVM.HandleOnCueShow))]
        [HarmonyPostfix]
        public static void DialogVM_HandleOnCueShow_Postfix(DialogVM __instance, CueShowData data)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var dialogName = Game.Instance.DialogController.Dialog?.name;
            Main.Multiplayer.OnAfterCueShow(dialogName, data.Cue.name, __instance.SystemAnswer.Value != null);
        }

        [HarmonyPatch(typeof(BookEventBaseController), nameof(BookEventBaseController.SetPage))]
        [HarmonyPostfix]
        public static void BookEventInterchapterController_SetPage_Postfix(BookEventBaseController __instance, BlueprintBookPage page, List<CueShowData> cues, List<BlueprintAnswer> answers)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Main.GetLogger<DialogController>().LogWarning("SET PAGE");
        }
    }
}
