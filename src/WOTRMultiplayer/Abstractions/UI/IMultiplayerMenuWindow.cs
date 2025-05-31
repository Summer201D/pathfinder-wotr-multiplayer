using System;
using System.Collections.Concurrent;
using WOTRMultiplayer.Abstractions.UI.Controllers.Menu;

namespace WOTRMultiplayer.Abstractions.UI
{
    public interface IMultiplayerMenuWindow
    {
        void AssignMenuItemControllers(IHostMenuItemController hostMenuItemController, IJoinMenuItemController joinMenuItemController);

        ConcurrentQueue<Action> MainThreadQueue { get; }
    }
}
