using System;

namespace WOTRMultiplayer.Abstractions.Unity
{
    public interface IMainThreadAccessor
    {
        void Post(Action action);
    }
}
