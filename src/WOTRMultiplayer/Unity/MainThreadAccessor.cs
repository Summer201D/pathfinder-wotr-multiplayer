using System;
using System.Collections.Concurrent;
using WOTRMultiplayer.Abstractions.Unity;

namespace WOTRMultiplayer.Unity
{
    public class MainThreadAccessor : IMainThreadAccessor
    {
        private ConcurrentQueue<Action> _mainThreadQueue;

        public void SetQueue(ConcurrentQueue<Action> mainThreadQueue)
        {
            _mainThreadQueue = mainThreadQueue;
        }

        public void Enqueue(Action action)
        {
            _mainThreadQueue.Enqueue(action);
        }
    }
}
