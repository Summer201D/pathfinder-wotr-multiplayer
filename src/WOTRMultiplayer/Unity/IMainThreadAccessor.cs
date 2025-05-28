using System;
using System.Collections.Concurrent;

namespace WOTRMultiplayer.Unity
{
    public interface IMainThreadAccessor
    {
        ConcurrentQueue<Action> MainThreadQueue { get; }

        void SetQueue(ConcurrentQueue<Action> mainThreadQueue);
    }
}
