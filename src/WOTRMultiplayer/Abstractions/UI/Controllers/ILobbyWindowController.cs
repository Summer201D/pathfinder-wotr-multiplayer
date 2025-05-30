using System.Collections.Generic;
using UnityEngine;
using WOTRMultiplayer.MP.Entities;
using WOTRMultiplayer.UI.Lobby;

namespace WOTRMultiplayer.Abstractions.UI.Controllers
{
    public interface ILobbyWindowController
    {
        void UpdatePlayers(List<NetworkPlayer> playersList);

        void InitializeContent(LobbyWindowOwner owner, Transform parent);
        void ResetData();
        void UpdateServerInfo(string serverAddress);
        void UpdateCharacters(List<string> portraits);
        void SetActiveOwner(LobbyWindowOwner owner);

        void ResetOwner(LobbyWindowOwner owner);
    }
}
