using System;
using WOTRMultiplayer.Entities;
using WOTRMultiplayer.Entities.Connectivity;

namespace WOTRMultiplayer.Abstractions
{
    public interface IMultiplayerClient : IMultiplayerActor
    {
        void Connect(string address, int port);

        void Connect(string code, string password, ExternalServer externalServer);

        bool IsConnecting { get; }

        Action OnNetworkError { get; set; }

        Action<NetworkCharacter> OnCharacterOwnerChanged { get; set; }

        void OnBeforeTryRollRestRandomEncounter();
    }
}
