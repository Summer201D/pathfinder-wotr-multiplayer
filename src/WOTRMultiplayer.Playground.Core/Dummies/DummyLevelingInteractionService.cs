using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Entities.Leveling;

namespace WOTRMultiplayer.Playground.Core.Dummies
{
    public class DummyLevelingInteractionService : ILevelingInteractionService
    {
        public void ChangeLevelingBirthDay(NetworkLevelingSequenceDirection direction)
        {
        }

        public void ChangeLevelingBirthMonth(NetworkLevelingSequenceDirection direction)
        {
        }

        public void ChangeLevelingRacialAbilityScoreBonus(NetworkLevelingSequenceDirection direction)
        {
        }

        public void CompleteLeveling()
        {
        }

        public void CompleteLevelingRespec()
        {
        }

        public void DecreaseLevelingAbilityScore(NetworkLevelingAbilityScore networkLevelingAbilityScore)
        {
        }

        public void DecreaseLevelingSkillPoint(NetworkLevelingSkillPoint networkLevelingSkillPoint)
        {
        }

        public string GetCurrentRespecWindowUnitId()
        {
            return string.Empty;
        }

        public void IncreaseLevelingAbilityScore(NetworkLevelingAbilityScore networkLevelingAbilityScore)
        {
        }

        public void IncreaseLevelingSkillPoint(NetworkLevelingSkillPoint networkLevelingSkillPoint)
        {
        }

        public void InitiateLevelingRespecLevelUp()
        {
        }

        public void InitiateLevelingRespecMythicLevelUp()
        {
        }

        public void RemoveLevelingSpell(NetworkLevelingSpell networkLevelingSpell)
        {
        }

        public void SelectLevelingAlignment(string alignmentId)
        {
        }

        public void SelectLevelingBodyColorAppearance(string textureName)
        {
        }

        public void SelectLevelingBodyTypeAppearance(int index)
        {
        }

        public void SelectLevelingClass(string classId)
        {
        }

        public void SelectLevelingClassArchetype(string archetypeId)
        {
        }

        public void SelectLevelingEyesColorAppearance(string textureName)
        {
        }

        public void SelectLevelingFaceAppearance(int index)
        {
        }

        public void SelectLevelingFeature(NetworkLevelingFeature networkLevelingFeature)
        {
        }

        public void SelectLevelingGender(string genderId)
        {
        }

        public void SelectLevelingHairColorAppearance(string textureName)
        {
        }

        public void SelectLevelingHairStyleAppearance(int index)
        {
        }

        public void SelectLevelingHornsAppearance(int index)
        {
        }

        public void SelectLevelingHornsColorAppearance(string textureName)
        {
        }

        public void SelectLevelingPortrait(NetworkLevelingPortrait levelingPortrait)
        {
        }

        public void SelectLevelingPrimaryOutfitColorAppearance(string textureName)
        {
        }

        public void SelectLevelingRace(string raceId)
        {
        }

        public void SelectLevelingScarAppearance(int index)
        {
        }

        public void SelectLevelingSecondaryOutfitColorAppearance(string textureName)
        {
        }

        public void SelectLevelingSpell(NetworkLevelingSpell networkLevelingSpell)
        {
        }

        public void SelectLevelingTattooAppearance(NetworkLevelingTattoo levelingTattoo)
        {
        }

        public void SelectLevelingTattooColorAppearance(NetworkLevelingTattoo levelingTattoo)
        {
        }

        public void SelectLevelingVoice(NetworkLevelingVoice levelingVoice)
        {
        }

        public void SelectLevelingWarpaintAppearance(NetworkLevelingWarpaint levelingWarpaint)
        {
        }

        public void SelectLevelingWarpaintColorAppearance(NetworkLevelingWarpaint levelingWarpaint)
        {
        }

        public void SelectMythicLevelingClass(string mythicClassId)
        {
        }

        public void SetLevelingName(string name)
        {
        }

        public void StartLeveling(string unitId, NetworkLevelingType levelingType)
        {
        }

        public void SwitchLevelingPhase(NetworkLevelingPhase networkLevelingPhase)
        {
        }

        public void TerminateLeveling()
        {
        }

        public void UpdateLevelingPhaseControls(bool isEnabled)
        {
        }

        public void UpdateLevelingRespecUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount)
        {
        }
    }
}
