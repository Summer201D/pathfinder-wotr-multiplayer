using System.Collections.Generic;
using Kingmaker.UI.MVVM._VM.SaveLoad;
using UnityEngine;
using WOTRMultiplayer.Entities;

namespace WOTRMultiplayer.Abstractions.UI.Controllers
{
    public interface ILobbyWindowController
    {
        void UpdatePlayers(List<NetworkPlayer> playersList);

        void InitializeContent(Transform parent);
        void Reset();
        void UpdateServerInfo(string serverAddress);
        void UpdateCharacters(SaveSlotVM value);
    }
}
