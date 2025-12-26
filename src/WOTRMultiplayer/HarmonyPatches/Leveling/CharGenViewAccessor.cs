using Kingmaker;
using Kingmaker.UI.MVVM._PCView.CharGen;
using Kingmaker.UI.MVVM._PCView.GlobalMap;
using Kingmaker.UI.MVVM._PCView.InGame;
using Kingmaker.UI.MVVM._PCView.MainMenu;

namespace WOTRMultiplayer.HarmonyPatches.Leveling
{
    public static class CharGenViewAccessor
    {
        public static CharGenContextPCView GetCharGenContextView()
        {
            var charGenContext = Game.Instance.RootUiContext.m_UIView switch
            {
                InGamePCView inGamePCView => inGamePCView.m_StaticPartPCView.m_CharGenContextPCView,
                GlobalMapPCView globalMapPCView => globalMapPCView.m_CharGenContextPCView,
                MainMenuPCView mainMenuPCView => mainMenuPCView.m_CharGenContextPCView,
                _ => null
            };

            return charGenContext;
        }
    }
}
