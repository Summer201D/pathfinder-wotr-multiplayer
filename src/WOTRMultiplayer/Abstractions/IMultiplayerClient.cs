using System;
using WOTRMultiplayer.Entities;

namespace WOTRMultiplayer.Abstractions
{
    public interface IMultiplayerClient : IMultiplayerActor
    {
        void Connect(string address, int port);

        bool IsConnecting { get; }

        Action OnNetworkError { get; set; }

        Action<NetworkCharacter> OnCharacterOwnerChanged { get; set; }

        void OnBeforeTryRollRestRandomEncounter();
    }
}
