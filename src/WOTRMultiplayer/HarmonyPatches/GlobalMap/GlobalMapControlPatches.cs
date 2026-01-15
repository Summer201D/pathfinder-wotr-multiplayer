using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.Globalmap;
using Kingmaker.Globalmap.View;
using Kingmaker.UI.MVVM._PCView.GlobalMap.Toolbar;
using Kingmaker.UI.MVVM._VM.Crusade.Armies;
using Kingmaker.UI.MVVM._VM.GlobalMap;
using Kingmaker.UI.MVVM._VM.GlobalMap.Toolbar;
using Microsoft.Extensions.Logging;
using UniRx;
using WOTRMultiplayer.Entities.GlobalMap;

namespace WOTRMultiplayer.HarmonyPatches.GlobalMap
{
    [HarmonyPatch]
    public class GlobalMapControlPatches
    {
        [HarmonyPatch(typeof(GlobalMapSelectController), nameof(GlobalMapSelectController.HandleClick), [typeof(GlobalMapPawn)])]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GlobalMapSelectController_HandleClick_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var lookFor = AccessTools.Method(typeof(GlobalMapController), nameof(GlobalMapController.SetSelectedArmy));
            var armyClickCall = AccessTools.Method(typeof(GlobalMapControlPatches), nameof(GlobalMapControlPatches.OnArmyPawnClicked));
            var playerClickCall = AccessTools.Method(typeof(GlobalMapControlPatches), nameof(GlobalMapControlPatches.OnPlayerPawnClicked));
            var matcher = new CodeMatcher(instructions);

            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<GlobalMapControlPatches>().LogError("Transpiler has not been applied (GlobalMapArmyPawnClick). Target={Target}", target);
                return instructions;
            }

            var armyPawnInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldloc_S, 4),
                new(OpCodes.Call, armyClickCall)
            };
            match = match.Advance(-1).Insert(armyPawnInstructions);

            match = matcher.End().SearchBackwards(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<GlobalMapControlPatches>().LogError("Transpiler has not been applied (GlobalMapPlayerPawnClick). Target={Target}", target);
                return instructions;
            }
            var playerPawnInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Call, playerClickCall)
            };
            match = match.Insert(playerPawnInstructions);

            Main.GetLogger<GlobalMapControlPatches>().LogInformation("Transpiler has been applied (GlobalMapArmyPawnClick + GlobalMapPlayerPawnClick). Target={Target}", target);
            return matcher.Instructions();
        }

        [HarmonyPatch(typeof(GlobalMapToolbarPCView), nameof(GlobalMapToolbarPCView.BindViewImplementation))]
        [HarmonyPostfix]
        public static void GlobalMapToolbarPCView_BindViewImplementation_Postfix(GlobalMapToolbarPCView __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            __instance.AddDisposable(__instance.CanSkipDay.Subscribe<bool>(value =>
            {
                __instance.m_SkipDay.Interactable = value && Main.Multiplayer.CanNavigateOnGlobalMap();
            }));

            var armyMode = GetArmyMode(__instance.ViewModel.ArmyMode.Value);
            // OnGlobalMapLoaded(armyMode)
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("GlobalMapToolbarPCView_BindViewImplementation_Postfix. ArmyMode={ArmyMode}", armyMode);
        }

        [HarmonyPatch(typeof(GlobalMapToolbarView<GlobalMapToolbarVM>), nameof(GlobalMapToolbarView<GlobalMapToolbarVM>.DestroyViewImplementation))]
        [HarmonyPrefix]
        public static void GlobalMapToolbarView_DestroyViewImplementation_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // OnGlobalMapDisposed - remove army mode counters?
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("GlobalMapToolbarView_DestroyViewImplementation_Prefix");
        }

        [HarmonyPatch(typeof(GlobalMapToolbarView<GlobalMapToolbarVM>), nameof(GlobalMapToolbarView<GlobalMapToolbarVM>.OnSkipDay))]
        [HarmonyPrefix]
        public static void GlobalMapToolbarView_OnSkipDay_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // OnGlobalMapSkipDay
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("GlobalMapToolbarView_OnSkipDay_Prefix");
        }

        [HarmonyPatch(typeof(GlobalMapToolbarVM), nameof(GlobalMapToolbarVM.ChangeArmyMode), [typeof(bool)])]
        [HarmonyPrefix]
        public static void GlobalMapToolbarVM_ChangeArmyMode_Prefix(bool state)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }


            // OnGlobalMapArmyModeChanged - Player=false Army=true
            // CurrentArmyMode: Player, Army
            var armyMode = GetArmyMode(state);
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("GlobalMapToolbarVM_ChangeArmyMode_Prefix. ArmyMode={ArmyMode}", armyMode);
        }

        [HarmonyPatch(typeof(GlobalMapToolbarSettingsVM), nameof(GlobalMapToolbarSettingsVM.SwitchAutoTacticalCombat))]
        [HarmonyPostfix]
        public static void GlobalMapToolbarSettingsVM_SwitchAutoTacticalCombat_Postfix(GlobalMapToolbarSettingsVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // OnGlobalMapAutoCrusadeCombatChanged - enforce always off?
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("GlobalMapToolbarSettingsVM_SwitchAutoTacticalCombat_Prefix. Value={Value}", __instance.UISettings.AutoTacticalCombat);
        }

        [HarmonyPatch(typeof(GlobalMapCrusadeArmyVM), nameof(GlobalMapCrusadeArmyVM.OnSelectClick))]
        [HarmonyPrefix]
        public static void GlobalMapCrusadeArmyVM_OnSelectClick_Prefix(GlobalMapCrusadeArmyVM __instance)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // OnGlobalMapSelectedArmyChanged(armyId) - force ArmyMode.Army ?
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("GlobalMapCrusadeArmyVM_OnSelectClick_Prefix. ArmyId={ArmyId}", __instance.Army?.Id);
        }

        private static void OnArmyPawnClicked(GlobalMapArmyPawn armyPawn)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // OnGlobalMapSelectedArmyChanged(armyId)  - force ArmyMode.Army ?
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("OnArmyPawnClicked. ArmyId={ArmyId}", armyPawn.State.Id);
        }

        private static void OnPlayerPawnClicked()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            // OnGlobalMapSelectedArmyChanged(null)  - force ArmyMode.Player ?
            Main.GetLogger<GlobalMapControlPatches>().LogWarning("OnPlayerPawnClicked");
        }

        private static NetworkGlobalMapArmyMode GetArmyMode(bool state)
        {
            if (state)
            {
                return NetworkGlobalMapArmyMode.Army;
            }

            return NetworkGlobalMapArmyMode.Player;
        }
    }
}
