using System;
using System.Collections.Generic;
using System.Net;
using WOTRMultiplayer.MP;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerHost
    {
        void Create(string gameName, List<string> portraits, MultiplayerSettings multiplayerSettings);

        void Dispose();

        bool ReadyChanged();

        void NotifyGameCharactersChanged(string saveGameName, List<string> portraits);

        void Start();

        bool IsInLobby { get; }

        bool IsActive { get; }

        Action<List<NetworkPlayer>> OnPlayersChanged { get; set; }

        Action<EndPoint> OnConnected { get; set; }
    }
}
