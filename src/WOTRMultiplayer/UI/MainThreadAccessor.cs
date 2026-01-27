using System;
using UniRx;
using WOTRMultiplayer.Abstractions.Unity;

namespace WOTRMultiplayer.UI
{
    public class MainThreadAccessor : IMainThreadAccessor
    {
        public void Post(Action action)
        {
            // TODO: review usages, a lot of calculations can be done outside on main thread
            MainThreadDispatcher.Post(x => action(), null);
        }
    }
}
