using System.Net;
using WOTRMultiplayer.Networking;
using WOTRMultiplayer.Networking.Messages.System;

namespace WOTRMultiplayer
{
    public class MultiplayerClient
    {
        private readonly NetworkServerClient _networkServerClient;

        public MultiplayerClient(NetworkServerClient networkServerClient)
        {
            _networkServerClient = networkServerClient;

            RegisterMessageHandlers();
        }

        public void Join(string address, MultiplayerSettings settings)
        {
            if (!Networking.Extensions.IPEndPoint.TryParse(address, out IPEndPoint endpoint))
            {
                return;
            }


            _networkServerClient.ConnectAsync(endpoint.Address.ToString(), endpoint.Port).Wait();
        }

        private void RegisterMessageHandlers()
        {
            _networkServerClient
                .Register<NetworkClientNameRequest>(OnNameRequested);
        }

        private void OnNameRequested(NetworkClientNameRequest request)
        {
            var nameResponse = new NetworkClientNameResponse() { Name = "AAA" };
            _networkServerClient.Send(nameResponse);
        }
    }
}
