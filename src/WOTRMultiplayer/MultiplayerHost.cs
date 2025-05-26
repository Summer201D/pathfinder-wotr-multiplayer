using WOTRMultiplayer.Networking;
using WOTRMultiplayer.Networking.Messages.System;

namespace WOTRMultiplayer
{
    public class MultiplayerHost
    {
        private readonly NetworkServer _networkServer;

        public bool IsActive => _networkServer.IsActive;

        public MultiplayerHost(NetworkServer networkServer)
        {
            _networkServer = networkServer;

            RegisterMessageHandlers();
        }

        public void Start(MultiplayerSettings settings)
        {

            _networkServer.Start(settings.NetworkInterfaceBinding, settings.Port);
        }

        private void RegisterMessageHandlers()
        {
            _networkServer.Register<NetworkClientNameResponse>(OnNetworkClientNameResponse);
        }

        private void OnNetworkClientNameResponse(NetworkClientNameResponse response)
        {
        }
    }
}
