using WOTRMultiplayer.Abstractions.UI;
using WOTRMultiplayer.MP.Entities;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayer
    {
        IUIFactory Factory { get; }

        bool InitializeMultiplayer(InitializeMultiplayerContext context);

        void TerminateMultiplayer();

        void InitializeEscMenuLobbyWindow(InitializeEscMenuLobbyWindowContext context);

        bool IsActive { get; }
    }
}
