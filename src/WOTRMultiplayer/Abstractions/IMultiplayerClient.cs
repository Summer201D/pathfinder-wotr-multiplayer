using System;
using WOTRMultiplayer.Entities;

namespace WOTRMultiplayer.Abstractions
{
    public interface IMultiplayerClient : IMultiplayerActor
    {
        AddressParseResult Connect(string address);

        bool IsConnecting { get; }

        Action OnNetworkError { get; set; }

        Action<NetworkCharacter> OnCharacterOwnerChanged { get; set; }

        void OnBeforeTryRollRestRandomEncounter();
    }
}
