using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Settings;
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

            const string GameAutopauseSettingsGroup = "GameAutopauseSettingsGroup";

            foreach (var settingsGroup in __instance.m_GameSettingsList)
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

                    var floatValue = FindValueBase<float>(settingEntity.GetType(), settingEntity);
                    if (floatValue != null)
                    {
                        floatValue.ManualModificationLock = true;
                        continue;
                    }
                    var boolValue = FindValueBase<bool>(settingEntity.GetType(), settingEntity);
                    if (boolValue != null)
                    {
                        boolValue.ManualModificationLock = true;
                        continue;
                    }
                    var entityValue = FindValueBase<EntitiesType>(settingEntity.GetType(), settingEntity);
                    if (entityValue != null)
                    {
                        entityValue.ManualModificationLock = true;
                    }
                }
            }
        }

        private static UISettingsEntityWithValueBase<T> FindValueBase<T>(Type type, object obj)
        {
            if (type == null)
            {
                return null;
            }

            if (type.GenericTypeArguments.Length == 0)
            {
                return FindValueBase<T>(type.BaseType, obj);
            }

            if (typeof(UISettingsEntityWithValueBase<T>).IsAssignableFrom(type))
            {
                return (UISettingsEntityWithValueBase<T>)obj;
            }

            return FindValueBase<T>(type.BaseType, obj);
        }
    }
}