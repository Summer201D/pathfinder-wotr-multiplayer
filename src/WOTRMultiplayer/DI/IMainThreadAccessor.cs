using System;
using System.Collections.Concurrent;

namespace WOTRMultiplayer.DI
{
    public interface IMainThreadAccessor
    {
        ConcurrentQueue<Action> MainThreadQueue { get; }

        void SetQueue(ConcurrentQueue<Action> mainThreadQueue);
    }
}
