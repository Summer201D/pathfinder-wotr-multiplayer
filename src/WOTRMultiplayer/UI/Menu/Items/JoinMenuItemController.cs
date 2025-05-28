using Microsoft.Extensions.Logging;
using Owlcat.Runtime.UI.Controls.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WOTRMultiplayer.Abstractions.MP;
using WOTRMultiplayer.Abstractions.UI;
using WOTRMultiplayer.Abstractions.UI.Controllers;
using WOTRMultiplayer.Abstractions.UI.Controllers.Menu;
using WOTRMultiplayer.Extensions;
using WOTRMultiplayer.UI.Lobby;

namespace WOTRMultiplayer.UI.Menu.Items
{
    public class JoinMenuItemController : MenuItemController, IJoinMenuItemController
    {
        public const string JoinMenuItemContentObjectName = "JoinMenuItemContent";
        public const string JoinLobbyScreenObjectName = "JoinLobbyScreen";
        public const string LobbyWindowObjectName = "MultiplayerLobby";

        private readonly ILogger<JoinMenuItemController> _logger;
        private readonly ILobbyWindowController _lobbyWindowController;
        private readonly IUIFactory _uIFactory;
        private readonly IMultiplayerClient _multiplayerClient;
        private GameObject _menuContent;

        protected override GameObject MenuContent => _menuContent;

        public JoinMenuItemController(
            ILogger<JoinMenuItemController> logger,
            ILobbyWindowController lobbyWindowController,
            IMultiplayerClient multiplayerClient,
            IUIFactory uIFactory)
            : base(logger)
        {
            _logger = logger;
            _lobbyWindowController = lobbyWindowController;
            _uIFactory = uIFactory;
            _multiplayerClient = multiplayerClient;
        }

        public override void Activate()
        {
            _logger.LogInformation("Trying to activate");

            if (IsActive)
            {
                return;
            }

            _lobbyWindowController.SetActiveOwner(LobbyWindowOwner.JoinMenu);
            base.Activate();
        }

        protected override void InitializeInternal(GameObject baseLayout)
        {
            var label = MenuItem.GetComponentInChildren<TextMeshProUGUI>();
            label.SetText(StringConsts.MultiplayerWindow.JoinMenuLabel);

            _menuContent = UnityEngine.Object.Instantiate(baseLayout, baseLayout.transform);
            _menuContent.name = JoinMenuItemContentObjectName;
            _menuContent.AddComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 25, 0);
            _menuContent.CleanupAllChildren();
            var menuContentRect = _menuContent.GetComponent<RectTransform>();
            menuContentRect.sizeDelta = new Vector2(menuContentRect.sizeDelta.x * 0.4f, menuContentRect.sizeDelta.y * 0.88f);

            var content = _uIFactory.CreateDefaultGameObject(_menuContent.transform);
            content.name = "JoinLobbyScreen";
            content.AddComponent<VerticalLayoutGroup>();

            var lobbyWindow = _uIFactory.CreateDefaultGameObject(content.transform);
            var aaaa = lobbyWindow.AddComponent<LayoutElement>();
            aaaa.preferredHeight = menuContentRect.sizeDelta.y;
            lobbyWindow.AddComponent<VerticalLayoutGroup>();
            lobbyWindow.name = "LobbyWindow";
            var lobbyWindowRect = lobbyWindow.GetComponent<RectTransform>();
            lobbyWindowRect.sizeDelta = menuContentRect.sizeDelta;
            _lobbyWindowController.InitializeContent(LobbyWindowOwner.JoinMenu, lobbyWindow.transform);

            var actionMenuContainer = _uIFactory.CreateDefaultGameObject(content.transform);
            actionMenuContainer.AddComponent<Image>().color = Color.red;

            actionMenuContainer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
            actionMenuContainer.AddComponent<HorizontalLayoutGroup>();
            var actionMenuContainerLayout = actionMenuContainer.AddComponent<LayoutElement>();
            actionMenuContainerLayout.preferredHeight = menuContentRect.sizeDelta.y * 0.05f;

            // input + button ?
            var joinLobbyControlsMenu = _uIFactory.CreateDefaultGameObject(actionMenuContainer.transform);
            joinLobbyControlsMenu.AddComponent<Image>().color = Color.green;
            joinLobbyControlsMenu.AddComponent<HorizontalLayoutGroup>();
            var serverInfoInputObject = _uIFactory.CreateInput(joinLobbyControlsMenu.transform);
            //serverInfoInputObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            serverInfoInputObject.transform.Find(UIFactory.InputPlaceholderObjectName)
                .GetComponent<TextMeshProUGUI>()
                .SetText(StringConsts.MultiplayerWindow.JoinMenu.ServerInputPlaceholder);

            var buttonObject = _uIFactory.CreateButton(joinLobbyControlsMenu.transform);
            var buttonObjectLayout = buttonObject.AddComponent<LayoutElement>();
            var buttonObjectSizeFitter = buttonObject.AddComponent<ContentSizeFitter>();
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().SetText(StringConsts.MultiplayerWindow.JoinMenu.JoinButtonLabel);
            var button = buttonObject.GetComponent<OwlcatButton>();
            button.OnLeftClick.AddListener(OnJoinButtonClicked);

            // leave + ready buttons?
            var lobbyControlsMenu = _uIFactory.CreateDefaultGameObject(actionMenuContainer.transform);
            lobbyControlsMenu.SetActive(false);
            lobbyControlsMenu.AddComponent<HorizontalLayoutGroup>();
        }

        private void OnJoinButtonClicked()
        {
            _logger.LogInformation("Join button clicked");
        }
    }
}
