using System;
using System.Net;
using System.Threading.Tasks;
using WOTRMultiplayer.Networking.Awaiters;

namespace WOTRMultiplayer.Networking.Abstractions
{
    public interface INetworkServer : INetworkReceiver
    {
        Action<long> OnClientConnected { get; set; }

        Action<long> OnClientDisconnected { get; set; }

        Action<EndPoint> OnServerStarted { get; set; }

        void Send(long clientId, object message);

        Task<T> SendAndWaitForAsync<T>(long clientId, IAwaitableRequest message)
            where T : IAwaitableResponse;

        void SendAll(object message);

        void SendAllExcept(long clientId, object message);

        void Start(int hostPortRangeStart, int hostPortRangeEnd, TimeSpan awaiterTimeout);

        void Reset();
    }
}
