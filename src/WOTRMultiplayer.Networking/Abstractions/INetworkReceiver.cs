using System;
using WOTRMultiplayer.Networking.Consuming;

namespace WOTRMultiplayer.Networking.Abstractions
{
    public interface INetworkReceiver
    {
        bool IsActive { get; }

        INetworkReceiver On<TMessage>(Action<long, TMessage> messageHandler, MessageHandlerPriority priority = MessageHandlerPriority.Default)
            where TMessage : class;
    }
}
