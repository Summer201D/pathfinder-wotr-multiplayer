using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Settings;
using Kingmaker.Utility;
using Microsoft.Extensions.Logging;
using Owlcat.Runtime.UI.VirtualListSystem;
using WOTRMultiplayer.UI.Settings;

namespace WOTRMultiplayer.HarmonyPatches.MenuPatches
{
    [HarmonyPatch]
    public class SettingsPCViewPatches
    {
        [HarmonyPatch(typeof(SettingsPCView), nameof(SettingsPCView.Initialize))]
        [HarmonyPostfix]
        public static void SettingsPCView_Initialize_Postfix(SettingsPCView __instance)
        {
            try
            {
                Main.Multiplayer.Factory.StoreDropdownPrefab(__instance.m_SettingsViews.m_SettingsEntityDropdownViewPrefab);
            }
            catch (Exception ex)
            {
                Main.GetLogger<SettingsPCViewPatches>().LogError(ex, "Unable to apply SettingsPCView patch");
                throw;
            }
        }

        [HarmonyPatch(typeof(SettingsPCView.SettingsViews), nameof(SettingsPCView.SettingsViews.InitializeVirtualList))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SettingsViews_InitializeVirtualList_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var matcher = new CodeMatcher(instructions);
            var lookFor = AccessTools.Method(typeof(VirtualListComponent), nameof(VirtualListComponent.Initialize));
            var initializeTemplatesCall = AccessTools.Method(typeof(SettingsPCViewPatches), nameof(SettingsPCViewPatches.InitializeCustomSettingElements));
            var match = matcher.SearchForward(x => x.Calls(lookFor));
            if (match.IsInvalid)
            {
                Main.GetLogger<SettingsVMPatches>().LogError("Transpiler has not been applied. Target={Target}", target);
                return matcher.Instructions();
            }

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, initializeTemplatesCall)
            };
            match.Insert(newInstructions);
            Main.GetLogger<SettingsVMPatches>().LogInformation("Transpiler has been applied. Target={Target}", target);
            return matcher.Instructions();
        }

        public static IVirtualListElementTemplate[] InitializeCustomSettingElements(IVirtualListElementTemplate[] templates, SettingsPCView.SettingsViews views)
        {
            var baseTemplatesCount = templates.Length;
            var view = Main.Multiplayer.Factory.InitializeInputSettingTemplate(views.m_SettingsEntityBoolViewPrefab.gameObject);

            IVirtualListElementTemplate[] allTemplates = [.. templates.Concat(
                new VirtualListElementTemplate<SettingsEntityStringInputVM>(view),
                new VirtualListElementTemplate<SettingsEntityIntInputVM>(view)
                )];

            Main.GetLogger<SettingsVMPatches>().LogInformation("Initialized Custom UI Settings templates. BaseTemplatesCount={BaseTemplatesCount}, AllTemplatesCount={AllTemplatesCount}", baseTemplatesCount, allTemplates.Length);
            return allTemplates;
        }
    }
}
