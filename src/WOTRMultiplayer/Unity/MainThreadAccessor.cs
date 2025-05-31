using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.Unity;

namespace WOTRMultiplayer.Unity
{
    public class MainThreadAccessor : IMainThreadAccessor
    {
        private ConcurrentQueue<Action> _mainThreadQueue;
        private readonly ILogger<MainThreadAccessor> _logger;

        public MainThreadAccessor(ILogger<MainThreadAccessor> logger)
        {
            _logger = logger;
        }

        public void SetQueue(ConcurrentQueue<Action> mainThreadQueue)
        {
            _mainThreadQueue = mainThreadQueue;
        }

        public void Enqueue(Action action)
        {
            if (_mainThreadQueue == null)
            {
                _logger.LogError("Main thread queue is not set");
                return;
            }

            _mainThreadQueue.Enqueue(action);
        }
    }
}
