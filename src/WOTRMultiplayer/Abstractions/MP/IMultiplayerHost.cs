using System.Collections.Generic;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerHost : IMultiplayerParticipant
    {
        void Create(string saveFilePath, List<NetworkCharacterOwnership> characters);

        void UpdateSaveGame(string saveFilePath, List<NetworkCharacterOwnership> characters);

        void Start();

        void ChangeCharacterOwner(int characterIndex, int playerIndex);

        void LeaveArea(string areaExitId);

        void SendSelectedAnswer();

        void UnitCommandDidEnd(NetworkUnitCommand networkCommand);
    }
}
