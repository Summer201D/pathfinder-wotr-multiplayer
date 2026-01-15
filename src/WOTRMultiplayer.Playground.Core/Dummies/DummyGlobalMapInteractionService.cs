using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Entities.GlobalMap;

namespace WOTRMultiplayer.Playground.Core.Dummies
{
    public class DummyGlobalMapInteractionService : IGlobalMapInteractionService
    {
        public void AcceptEncounter()
        {
        }

        public void AvoidEncounter()
        {
        }

        public void CloseIngredientCollection()
        {
        }

        public void CloseMessageBox()
        {
        }

        public void CollectIngredients(NetworkGlobalMapLocation globalMapLocation)
        {
        }

        public void ContinueTravel(NetworkGlobalMapState globalMapState)
        {
        }

        public void EnterLocation(NetworkGlobalMapLocation globalMapLocation)
        {
        }

        public bool IsAtLocation(NetworkGlobalMapLocation globalMapLocation)
        {
            return false;
        }

        public void OpenRestMenu()
        {
        }

        public void RollEncounter(NetworkGlobalMapEncounter encounter)
        {
        }

        public void SkipDay()
        {
        }

        public void StartTravel(NetworkGlobalMapLocation destination)
        {
        }

        public void StopTravel(NetworkGlobalMapState globalMapState)
        {
        }

        public void UpdateEncounterMessageUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount)
        {
        }

        public void UpdateIngredientCollectionUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount)
        {
        }

        public void UpdateMessageBoxUI(bool isInteractable, int readyPlayersCount, int totalPlayersCount)
        {
        }

        public void UpdateUIState(bool isInteractable, int readyPlayersCount, int totalPlayersCount)
        {
        }
    }
}
