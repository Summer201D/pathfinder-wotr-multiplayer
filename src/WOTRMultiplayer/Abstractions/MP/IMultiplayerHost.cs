using System.Collections.Generic;
using Kingmaker.EntitySystem.Persistence;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerHost : IMultiplayerParticipant
    {
        void Create(SaveInfo save, List<NetworkCharacterOwnership> characters);

        void UpdateSaveGame(SaveInfo save, List<NetworkCharacterOwnership> characters);

        void Start();

        void ChangeCharacterOwner(int characterIndex, int playerIndex);

        void LeaveArea(string areaExitId);

        void SendSelectedAnswer();
    }
}
