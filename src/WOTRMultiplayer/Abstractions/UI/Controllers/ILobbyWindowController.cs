using System.Collections.Generic;
using System.Net;
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
        void UpdateServerInfo(EndPoint point);
        void UpdateCharacters(SaveSlotVM value);
    }
}
