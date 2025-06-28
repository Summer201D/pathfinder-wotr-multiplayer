using System.Collections.Generic;
using Kingmaker.EntitySystem.Persistence;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerHost : IMultiplayerParticipant
    {
        void Create(SaveInfo save, List<NetworkCharacter> characters);

        void UpdateSaveGame(SaveInfo save, List<NetworkCharacter> characters);

        void Start();

        void ChangeCharacterOwner(int characterIndex, int playerIndex);
    }
}
