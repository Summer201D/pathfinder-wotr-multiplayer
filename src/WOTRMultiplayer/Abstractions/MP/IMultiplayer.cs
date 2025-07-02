using Kingmaker.Controllers.Clicks.Handlers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
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

        void MoveCharacter(UnitEntityData unit, ClickGroundHandler.CommandSettings settings);

        bool CanControlCharacter(string characterName);
        bool StartGameMode(GameModeType type);
        bool StopGameMode(GameModeType type);
        bool CanLeaveArea();

        bool IsActive { get; }
    }
}
