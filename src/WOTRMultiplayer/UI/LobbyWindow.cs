using Kingmaker.UI.ServiceWindow;

namespace WOTRMultiplayer.UI
{
    public class LobbyWindow : UIWindow
    {
        // title
        // vertical list of players, ready indicator
        // x6 portraits /slots
        // x6 dropdowns
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnHide()
        {
            Logging.Logger.Info($"Hide {nameof(LobbyWindow)}");
        }

        public override void OnShow()
        {
            Logging.Logger.Info($"Show {nameof(LobbyWindow)}");
        }
    }
}
