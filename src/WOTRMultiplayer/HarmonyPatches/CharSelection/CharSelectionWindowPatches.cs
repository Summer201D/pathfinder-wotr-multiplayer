using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.CharSelect;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.CharSelection
{
    [HarmonyPatch]
    public class CharSelectionWindowPatches
    {
        [HarmonyPatch(typeof(CharSelectWindow), nameof(CharSelectWindow.ShowWindow))]
        [HarmonyPrefix]
        public static void CharSelectWindow_ShowWindow_Prefix(CharSelectWindow __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            if (!__instance.m_IsShowed)
            {
                Main.GetLogger<CharSelectionWindowPatches>().LogInformation("Added OnButtonClose subscription");
                Game.Instance.UI.EscManager.Subscribe(__instance.OnButtonClose);
            }
        }

        [HarmonyPatch(typeof(CharSelectWindow), nameof(CharSelectWindow.ShowWindow))]
        [HarmonyPostfix]
        public static void CharSelectWindow_ShowWindow_Postfix(CharSelectWindow __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // no harm to call it multiple times
            Game.Instance.UI.EscManager.Unsubscribe(__instance.CloseWindow);

            Main.Multiplayer.OnCharacterSelectionWindowShown();
        }

        [HarmonyPatch(typeof(CharSelectWindow), nameof(CharSelectWindow.OnToggleSelected))]
        [HarmonyPostfix]
        public static void CharSelectWindow_OnToggleSelected_Postfix(CharSelectWindow __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var unitId = __instance.CurrentCharacter?.UniqueId;
            Main.Multiplayer.OnCharacterSelectionToggleChanged(unitId);
        }

        [HarmonyPatch(typeof(CharSelectWindow), nameof(CharSelectWindow.OnButtonOk))]
        [HarmonyPrefix]
        public static void CharSelectWindow_OnButtonOk_Prefix(CharSelectWindow __instance)
        {
            if (!Main.Multiplayer.IsActive || __instance.CurrentCharacter == null)
            {
                return;
            }

            Main.Multiplayer.OnCharacterSelectionWindowAccepted();
        }

        [HarmonyPatch(typeof(CharSelectWindow), nameof(CharSelectWindow.CloseWindow))]
        [HarmonyPrefix]
        public static void CharSelectWindow_CloseWindow_Prefix(CharSelectWindow __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            Game.Instance.UI.EscManager.Unsubscribe(__instance.OnButtonClose);
            Main.GetLogger<CharSelectionWindowPatches>().LogInformation("Removed OnButtonClose subscription");
        }

        [HarmonyPatch(typeof(CharSelectWindow), nameof(CharSelectWindow.OnButtonClose))]
        [HarmonyPrefix]
        public static bool CharSelectWindow_OnButtonClose_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanControlCharacterSelectionWindow();
            if (canContinue)
            {
                Main.Multiplayer.OnCharacterSelectionWindowClosed();
            }

            return canContinue;
        }
    }
}
