using System;
using System.Collections.Concurrent;

namespace WOTRMultiplayer.Abstractions.Unity
{
    public interface IMainThreadAccessor
    {
        void SetQueue(ConcurrentQueue<Action> mainThreadQueue);

        void Enqueue(Action action);
    }
}
