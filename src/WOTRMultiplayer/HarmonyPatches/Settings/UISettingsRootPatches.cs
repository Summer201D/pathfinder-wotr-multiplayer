using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.UI.SettingsUI;

namespace WOTRMultiplayer.HarmonyPatches.Settings
{
    [HarmonyPatch]
    public class UISettingsManagerPatches
    {
        private static readonly HashSet<string> UnmodifiableValues = new(
            [
                "GameMainSettingsGroup.AcceleratedMove",
                "GameMainSettingsGroup.AllowLootingInCombat",
                "TurnBased.EnableTurnBased",
                "TurnBased.AutoEndTurn",
                "TurnBased.AutoStopAfterFirstMoveAction",
                "TurnBased.TimeScaleInPlayerTurn",
                "TurnBased.TimeScaleInNonPlayerTurn",
            ],
            StringComparer.OrdinalIgnoreCase);

        [HarmonyPatch(typeof(UISettingsManager), nameof(UISettingsManager.Initialize))]
        [HarmonyPostfix]
        public static void UISettingsManager_Initialize_Postfix(UISettingsManager __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            UpdateGameSettings(__instance);
            UpdateDifficultySettings(__instance);
        }

        private static void UpdateDifficultySettings(UISettingsManager uiSettingsManager)
        {
            foreach (var settingsGroup in uiSettingsManager.m_DifficultySettingsList)
            {
                foreach (var settingEntity in settingsGroup.SettingsList)
                {
                    DisableSetting(settingEntity);
                }
            }
        }

        private static void UpdateGameSettings(UISettingsManager uiSettingsManager)
        {
            const string GameAutopauseSettingsGroup = "GameAutopauseSettingsGroup";

            foreach (var settingsGroup in uiSettingsManager.m_GameSettingsList)
            {
                var settingsGroupName = settingsGroup.name;
                foreach (var settingEntity in settingsGroup.SettingsList)
                {
                    var settingName = settingEntity.name;
                    var fullSettingName = $"{settingsGroupName}.{settingName}";
                    if (settingsGroupName != GameAutopauseSettingsGroup && !UnmodifiableValues.Contains(fullSettingName))
                    {
                        continue;
                    }

                    DisableSetting(settingEntity);
                }
            }
        }

        private static void DisableSetting(UISettingsEntityBase settingEntity)
        {
            var field = settingEntity.GetType().GetField(nameof(UISettingsEntityWithValueBase<object>.ManualModificationLock));
            field?.SetValue(settingEntity, true);
        }
    }
}