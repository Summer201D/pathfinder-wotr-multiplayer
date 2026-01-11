using System;
using System.Net;
using System.Threading.Tasks;
using WOTRMultiplayer.Networking.Awaiters;

namespace WOTRMultiplayer.Networking.Abstractions
{
    public interface INetworkClient : INetworkReceiver
    {
        bool IsActive { get; }

        bool IsConnecting { get; }

        Task ConnectAsync(string host, int port, TimeSpan awaiterTimeout);

        void Send(object message);

        /// <summary>
        /// make sure to block main thread for every call (.Result)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<T> SendAndWaitForAsync<T>(IAwaitableRequest message)
            where T : IAwaitableResponse;

        Action<Exception> OnError { get; set; }

        Action<EndPoint> OnConnected { get; set; }

        void Reset();
    }
}
