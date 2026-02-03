using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases.Class;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
using WOTRMultiplayer.Entities.Leveling;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    [HarmonyPatch]
    public class CharGenClassPatches
    {
        [HarmonyPatch(typeof(CharGenClassSelectorItemPCView), nameof(CharGenClassSelectorItemPCView.OnClick))]
        [HarmonyPrefix]
        public static bool CharGenClassSelectorItemPCView_OnClick_Prefix()
        {
            if (!Main.Multiplayer.IsActive)
            {
                return true;
            }

            var canContinue = Main.Multiplayer.CanMakeLevelingDecisions();
            return canContinue;
        }

        [HarmonyPatch(typeof(CharGenClassPhaseVM), nameof(CharGenClassPhaseVM.TryWarnChangeRace))]
        [HarmonyPostfix]
        public static void CharGenClassPhaseVM_TryWarnChangeRace_Postfix(CharGenClassSelectorItemVM archetypeVM)
        {
            if (!Main.Multiplayer.IsActive)
            {
                return;
            }

            var archetype = archetypeVM == null ? null : new NetworkLevelingArchetype { Id = archetypeVM.Archetype.AssetGuid.ToString(), Name = archetypeVM.Archetype.name };
            Main.Multiplayer.OnLevelingClassArchetypeSelected(archetype);
        }

        [HarmonyPatch(typeof(CharGenClassPhaseVM), nameof(CharGenClassPhaseVM.OnMechanicClassSelected))]
        [HarmonyPostfix]
        public static void CharGenClassPhaseVM_OnMechanicClassSelected_Postfix(BlueprintCharacterClass selectedClass)
        {
            if (selectedClass == null)
            {
                return;
            }

            var levelingClass = new NetworkLevelingClass
            {
                Id = selectedClass.AssetGuid.ToString(),
                Name = selectedClass.name
            };
            Main.Multiplayer.OnLevelingClassSelected(levelingClass);
        }
    }
}
