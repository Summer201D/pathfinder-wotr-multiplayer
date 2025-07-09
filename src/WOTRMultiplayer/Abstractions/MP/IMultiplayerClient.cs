using System;
using System.Collections.Generic;
using WOTRMultiplayer.MP.Entities;
using WOTRMultiplayer.MP.Entities.Rolls;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerClient : IMultiplayerParticipant
    {
        ConnectLobbyResult Connect(string address);

        NetworkDiceRoll GetHostRoll(int rollId);

        bool IsConnecting { get; }

        Action<string> OnNetworkError { get; set; }

        Action<List<NetworkCharacterOwnership>> OnGameCharactersChanged { get; set; }

        Action<int, int> OnCharacterOwnerChanged { get; set; }
    }
}
