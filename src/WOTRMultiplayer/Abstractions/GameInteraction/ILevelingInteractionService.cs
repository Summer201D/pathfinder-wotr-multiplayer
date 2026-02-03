using WOTRMultiplayer.Entities.Leveling;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface ILevelingInteractionService
    {
        void StartLeveling(string unitId, NetworkLevelingType levelingType);

        void SelectLevelingClassArchetype(NetworkLevelingArchetype levelingArchetype);

        void SelectLevelingClass(NetworkLevelingClass levelingClass);

        void SelectMythicLevelingClass(string mythicClassId);

        void SelectLevelingPortrait(NetworkLevelingPortrait levelingPortrait);

        void SelectLevelingVoice(NetworkLevelingVoice levelingVoice);

        void SelectLevelingGender(string genderId);

        void SelectLevelingRace(string raceId);

        void UpdateLevelingPhaseControls(bool isEnabled);

        void SwitchLevelingPhase(NetworkLevelingPhase networkLevelingPhase);

        void DecreaseLevelingSkillPoint(NetworkLevelingSkillPoint networkLevelingSkillPoint);

        void IncreaseLevelingSkillPoint(NetworkLevelingSkillPoint networkLevelingSkillPoint);

        void DecreaseLevelingAbilityScore(NetworkLevelingAbilityScore networkLevelingAbilityScore);

        void IncreaseLevelingAbilityScore(NetworkLevelingAbilityScore networkLevelingAbilityScore);

        void SelectLevelingAlignment(string alignmentId);

        void SetLevelingName(string name);

        void ChangeLevelingRacialAbilityScoreBonus(NetworkLevelingSequenceDirection direction);

        void ChangeLevelingBirthDay(NetworkLevelingSequenceDirection direction);

        void ChangeLevelingBirthMonth(NetworkLevelingSequenceDirection direction);

        void SelectLevelingFeature(NetworkLevelingFeature networkLevelingFeature);

        void SelectLevelingSpell(NetworkLevelingSpell networkLevelingSpell);

        void RemoveLevelingSpell(NetworkLevelingSpell networkLevelingSpell);

        void SelectLevelingWarpaintColorAppearance(NetworkLevelingWarpaint levelingWarpaint);

        void SelectLevelingWarpaintAppearance(NetworkLevelingWarpaint levelingWarpaint);

        void SelectLevelingTattooColorAppearance(NetworkLevelingTattoo levelingTattoo);

        void SelectLevelingTattooAppearance(NetworkLevelingTattoo levelingTattoo);

        void SelectLevelingScarAppearance(int index);

        void SelectLevelingBodyTypeAppearance(int index);

        void SelectLevelingSecondaryOutfitColorAppearance(string textureName);

        void SelectLevelingPrimaryOutfitColorAppearance(string textureName);

        void SelectLevelingHornsColorAppearance(string textureName);

        void SelectLevelingHornsAppearance(int index);

        void SelectLevelingHairStyleAppearance(int index);

        void SelectLevelingHairColorAppearance(string textureName);

        void SelectLevelingFaceAppearance(int index);

        void SelectLevelingEyesColorAppearance(string textureName);

        void SelectLevelingBodyColorAppearance(string textureName);

        void CompleteLeveling();

        void TerminateLeveling();

        void UpdateLevelingRespecUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount);

        void CompleteLevelingRespec();

        string GetCurrentRespecWindowUnitId();

        void InitiateLevelingRespecLevelUp();

        void InitiateLevelingRespecMythicLevelUp();
    }
}
