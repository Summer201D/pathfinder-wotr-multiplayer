using System;
using System.Collections.Concurrent;

namespace WOTRMultiplayer.DI
{
    public class MainThreadAccessor : IMainThreadAccessor
    {
        public ConcurrentQueue<Action> MainThreadQueue { get; private set; }

        public void SetQueue(ConcurrentQueue<Action> mainThreadQueue)
        {
            MainThreadQueue = mainThreadQueue;
        }
    }
}
