using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Kingmaker.BarkBanters;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Rest.State;
using Kingmaker.Utility;
using Microsoft.Extensions.Logging;

namespace WOTRMultiplayer.HarmonyPatches.Rest
{
    [HarmonyPatch]
    public class BarkBanterHelperPatches
    {
        /// <summary>
        /// WeightedRandom is replaced with ordering by banter asset id
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyPatch(typeof(BarkBanterHelper), nameof(BarkBanterHelper.SelectBanter))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BarkBanterHelper_SelectBanter_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var target = PatchesUtils.GetTranspilerTarget(MethodBase.GetCurrentMethod());
            var replaceWith = AccessTools.Method(typeof(BarkBanterHelperPatches), nameof(SelectBanter));
            var lookFor = AccessTools.PropertyGetter(typeof(BlueprintRoot), nameof(BlueprintRoot.Instance));
            var matcher = new CodeMatcher(instructions);
            var match = matcher.Start().RemoveInstructions(19); // linq class wrapper takes a lot

            var newInstructions = new List<CodeInstruction>()
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, replaceWith),
            };
            match.Insert(newInstructions);
            Main.GetLogger<PlaceRestMarkerHandlerPatches>().LogInformation("Transpiler has been applied. Target={target}", target);
            return matcher.Instructions();
        }

        public static BlueprintBarkBanter SelectBanter(TimeSpan campTime)
        {
            var units = CampingState.GetAllUnitsForRest(true, false);
            var allBanters = BlueprintRoot.Instance.Camping.AllBanters.Where((BlueprintBarkBanter b) => b.CanBePlayed(campTime, units)).ToList();
            if (!Main.Multiplayer.IsActive)
            {
                return allBanters.WeightedRandom<BlueprintBarkBanter>();
            }

            var selectedBanter = allBanters.OrderBy(x => x.AssetGuid).FirstOrDefault();
            var firstPhraseKey = selectedBanter?.FirstPhrase?.FirstOrDefault()?.Key;
            Main.GetLogger<BarkBanterHelperPatches>().LogInformation("Selected camping banter. Id={banterId}, FirstPhraseKey={firstPhraseKey}", selectedBanter?.AssetGuid, firstPhraseKey);

            return selectedBanter;
        }
    }
}
