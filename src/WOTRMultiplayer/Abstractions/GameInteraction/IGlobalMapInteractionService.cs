using WOTRMultiplayer.Entities.GlobalMap;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IGlobalMapInteractionService
    {
        bool IsAtLocation(NetworkGlobalMapLocation globalMapLocation);

        void ContinueTravel(NetworkGlobalMapState globalMapState);

        void StopTravel(NetworkGlobalMapState globalMapState);

        void UpdateMessageBoxUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount);

        void UpdateIngredientCollectionUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount);

        void UpdateEncounterMessageUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount);

        void UpdateUIState(bool isInteractable, int readyPlayersCount, int totalPlayersCount);

        void CollectIngredients(NetworkGlobalMapLocation globalMapLocation);

        void EnterLocation(NetworkGlobalMapLocation globalMapLocation);

        void AvoidEncounter();

        void AcceptEncounter();

        void RollEncounter(NetworkGlobalMapEncounter globalMapEncounter);

        void OpenRestMenu();

        void StartTravel(NetworkGlobalMapLocation destination);

        void CloseMessageBox();

        void CloseIngredientCollection();

        void SkipDay();
    }
}
