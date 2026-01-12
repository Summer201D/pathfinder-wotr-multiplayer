using System;
using System.Collections.Generic;
using UnityEngine;
using WOTRMultiplayer.Entities;
using WOTRMultiplayer.UI.Windows;

namespace WOTRMultiplayer.Abstractions.UI.Controllers
{
    public interface ILobbyWindowController
    {
        void UpdatePlayers(List<NetworkPlayer> players);

        void InitializeContent(LobbyWindowOwner owner, Transform parent);

        void ResetData();

        void UpdateServerInfo(NetworkGameConnectivity connectivity);

        void UpdateCharacters(List<NetworkCharacter> characters, bool isDropdownInteractable);

        void UpdateCharacterOwnerDropdown(NetworkCharacter character, bool silent = false);

        void SetActiveOwner(LobbyWindowOwner owner);

        void ResetOwnerContent(LobbyWindowOwner owner);

        Action<NetworkCharacter, NetworkPlayer> OnCharacterOwnerChanged { get; set; }
    }
}
