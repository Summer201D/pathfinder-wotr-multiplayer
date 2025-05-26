using System;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._PCView.ContextMenu;
using Kingmaker.UI.MVVM._VM.ContextMenu;
using UnityEngine;
using WOTRMultiplayer.UI;

namespace WOTRMultiplayer
{
    public class Multiplayer : IDisposable
    {
        private UI.Menu.MultiplayerWindow _multiplayerWindow;
        private readonly MultiplayerClient _multiplayerClient;
        private readonly MultiplayerHost _multiplayerHost;

        public UIFactory Factory { get; private set; }

        public Multiplayer(UIFactory uiFactory, MultiplayerHost multiplayerHost, MultiplayerClient multiplayerClient)
        {
            Factory = uiFactory;
            _multiplayerHost = multiplayerHost;
            _multiplayerClient = multiplayerClient;
        }

        public bool InjectMultiplayerMenuWindow(GameObject menuItemPrototype, Transform parent)
        {
            var multiplayerMenu = UnityEngine.Object.Instantiate(menuItemPrototype, parent);
            multiplayerMenu.transform.SetSiblingIndex(menuItemPrototype.transform.GetSiblingIndex());
            var multiplayerMenuView = multiplayerMenu.GetComponent<ContextMenuEntityPCView>();
            var element = Factory.CreateCopyOfCreditsScreen();
            _multiplayerWindow = element.AddComponent<UI.Menu.MultiplayerWindow>();
            _multiplayerWindow.Initialize();
            var text = UIUtility.GetSaberBookFormat(StringConsts.MainMenu.MultiplayerMenu);
            var viewModel = new ContextMenuEntityVM(new ContextMenuCollectionEntity(UIUtility.GetSaberBookFormat(text), ShowMultiplayerWindow));
            multiplayerMenuView.Bind(viewModel);
            return true;
        }

        private void ShowMultiplayerWindow()
        {
            _multiplayerWindow.Show(true);
        }

        public void Dispose()
        {
        }
    }
}
