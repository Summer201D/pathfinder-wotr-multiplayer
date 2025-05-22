using System;
using System.Collections.Generic;
using DG.Tweening;
using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.Common;
using Kingmaker.UI.FullScreenUITypes;
using Kingmaker.UI.MVVM;
using Kingmaker.UI.MVVM._PCView.Common;
using Kingmaker.UI.MVVM._PCView.ContextMenu;
using Kingmaker.UI.MVVM._PCView.MainMenu;
using Kingmaker.UI.MVVM._PCView.SaveLoad;
using Kingmaker.UI.MVVM._VM.ContextMenu;
using Kingmaker.UI.MVVM._VM.SaveLoad;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Abilities;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.UI.ServiceWindow.Credits;
using Owlcat.Runtime.UI.Controls.Button;
using Owlcat.Runtime.UI.VirtualListSystem;
using TMPro;
using UnityEngine;
using WOTRMultiplayer.Extensions;
using WOTRMultiplayer.Strings;

namespace WOTRMultiplayer.Menu
{
    [HarmonyPatch]
    public class MenuPatches
    {
        private static MultiplayerWindow _mainMenuMultiplayerWindow;

        private static SaveLoadPCView _cachedSaveLoadPCView;

        [HarmonyPatch(typeof(MainMenuSideBarPCView), "BindViewImplementation")]
        [HarmonyPrefix]
        public static void MainMenuSideBarPCView_BindViewImplementation_Prefix(MainMenuSideBarPCView __instance)
        {
            Logging.Logger.Info("Applying");
            try
            {
                var menuButtons = __instance.transform.GetChild(0);
                var menuItemToCopy = menuButtons.GetChild(3).gameObject;
                var multiplayerMenu = UnityEngine.Object.Instantiate(menuItemToCopy, menuButtons.transform);
                multiplayerMenu.transform.SetSiblingIndex(3);
                var multiplayerMenuView = multiplayerMenu.GetComponent<ContextMenuEntityPCView>();
                var window = GetOrCreateMultiplayerWindow();
                var text = UIUtility.GetSaberBookFormat(StringConsts.MainMenu.MultiplayerMenu);
                var viewModel = new ContextMenuEntityVM(new ContextMenuCollectionEntity(UIUtility.GetSaberBookFormat(text), () => window.Show(true)));
                multiplayerMenuView.Bind(viewModel);
            }
            catch (Exception ex)
            {
                Logging.Logger.Error("Unable to apply patch", ex);
                throw;
            }
        }

        private static MultiplayerWindow GetOrCreateMultiplayerWindow()
        {
            if (_mainMenuMultiplayerWindow != null)
            {
                Logging.Logger.Info($"Reusing existing instance of {nameof(MultiplayerWindow)}");
                return _mainMenuMultiplayerWindow;
            }

            Logging.Logger.Info($"Creating new instance of {nameof(MultiplayerWindow)}");

            var copy = UnityEngine.Object.Instantiate(Game.Instance.UI.CreditsUI.gameObject, Game.Instance.UI.MainMenu.transform);
            var originalWindow = copy.GetComponent<CreditsUIWindow>();
            _mainMenuMultiplayerWindow = copy.AddComponent<MultiplayerWindow>();
            UnityEngine.Object.DestroyImmediate(originalWindow);
            _mainMenuMultiplayerWindow.Initialize();
            return _mainMenuMultiplayerWindow;
        }

        public class HostMenuItemController : MenuItemController
        {
            private SaveLoadVM _saveLoadViewModel;
            private bool _setupLayout = true;

            public HostMenuItemController(MultiplayerWindow multiplayerWindow, GameObject menuItem, GameObject menuContent)
                : base(multiplayerWindow, menuItem, menuContent)
            {
                var label = menuItem.GetComponentInChildren<TextMeshProUGUI>();
                label.SetText(StringConsts.MultiplayerWindow.HostMenuLabel);
            }

            public override void Activate()
            {
                Logging.Logger.Info($"Trying to activate {nameof(HostMenuItemController)}. IsActive={IsActive}");

                if (IsActive)
                {
                    return;
                }

                var saveLoad = MenuContent.transform.GetChild(0).GetComponent<SaveLoadPCView>();
                _saveLoadViewModel = new SaveLoadVM(SaveLoadMode.Load, true, OnCloseSaveLoadVM, RootUIContext.Instance.CommonVM);

                if (_setupLayout)
                {
                    /// overriding save/load/delete buttons prefab to make sure original loadsave screen is not affected
                    var screen = saveLoad.gameObject.transform.Find("SaveLoadScreen");
                    var collectionView = screen.Find("SaveSlotCollectionPlace").Find("SaveSlotVirtualCollectionView");
                    var virtualView = collectionView.GetComponent<SaveSlotCollectionVirtualView>();
                    var prefab = virtualView.m_SaveSlotPrefab as SaveSlotPCView;
                    var copyPrefabObj = UnityEngine.Object.Instantiate(prefab.gameObject, prefab.transform.parent);
                    var newPrefab = copyPrefabObj.GetComponent<SaveSlotPCView>();
                    virtualView.m_VirtualList.Initialize(new VirtualListElementTemplate<ExpandableTitleVM>(virtualView.m_ExpandableTitleView), new VirtualListElementTemplate<SaveSlotVM>(newPrefab));
                    UnityEngine.Object.DestroyImmediate(newPrefab.m_SaveLoadButton.gameObject);
                    UnityEngine.Object.DestroyImmediate(newPrefab.m_DeleteButton.gameObject);
                    ///
                }

                // it's important to be called inbetween
                saveLoad.Bind(_saveLoadViewModel);

                if (_setupLayout)
                {
                    CleanupLoadSaveGamesLayout(saveLoad);
                }

                _setupLayout = false;
                saveLoad.Show();
                base.Activate();
            }

            public override void Deactivate()
            {
                DisposeSaveLoadVM();
                base.Deactivate();
            }

            private void DisposeSaveLoadVM()
            {
                _saveLoadViewModel?.Dispose();
            }

            private void OnCloseSaveLoadVM()
            {
                Window.OnCloseClicked();
            }

            private void CleanupLoadSaveGamesLayout(SaveLoadPCView saveLoadView)
            {
                UnityEngine.Object.DestroyImmediate(saveLoadView.gameObject.transform.Find("BackgroundWorldCover").gameObject);
                UnityEngine.Object.DestroyImmediate(saveLoadView.gameObject.transform.Find("Background").gameObject);
                var screen = saveLoadView.gameObject.transform.Find("SaveLoadScreen");
                var top = screen.Find("Top");
                UnityEngine.Object.DestroyImmediate(top.gameObject);

                var saveLoadDetails = screen.Find("SaveLoadDetails");
                var picture = saveLoadDetails.Find("Picture");
                picture.gameObject.SetActive(false);

                var buttons = saveLoadDetails.Find("Info").Find("Buttons");
                // TBD random buttons as placeholders
                var baseButton = buttons.Find("OwlcatButton").gameObject;
                var layout = baseButton.GetComponent<RectTransform>();
                layout.sizeDelta = new Vector2(layout.sizeDelta.x * 0.92f, layout.sizeDelta.y);
                var buttonCopy1 = UnityEngine.Object.Instantiate(baseButton, buttons);
                buttonCopy1.name = "Button1";
                var buttonCopy2 = UnityEngine.Object.Instantiate(baseButton, buttons);
                buttonCopy2.name = "Button2";
                var buttonCopy3 = UnityEngine.Object.Instantiate(baseButton, buttons);
                buttonCopy3.name = "Button3";
                buttons.gameObject.CleanupAllChildren(
                    x => x.name != "DlcRequiredLabel" && x.name != buttonCopy1.name && x.name != buttonCopy2.name && x.name != buttonCopy3.name);
                buttonCopy1.GetComponentInChildren<TextMeshProUGUI>().text = "Host";
                buttonCopy2.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
                buttonCopy3.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
            }
        }

        public class JoinMenuItemController : MenuItemController
        {
            public JoinMenuItemController(MultiplayerWindow multiplayerWindow, GameObject menuItem, GameObject menuContent)
                : base(multiplayerWindow, menuItem, menuContent)
            {
                var label = menuItem.GetComponentInChildren<TextMeshProUGUI>();
                label.SetText(StringConsts.MultiplayerWindow.JoinMenuLabel);
            }

            public override void Activate()
            {
                base.Activate();
            }
        }

        public class MenuItemController
        {
            private const string SelectedGameObjectName = "SelectedImage";
            private const string HoverGameObjectName = "HoverImage";

            private bool _isInitialized = false;
            private OwlcatButton Button => MenuItem.gameObject.GetComponent<OwlcatButton>();
            private GameObject _hoverImage;

            public GameObject MenuItem { get; private set; }
            public GameObject MenuContent { get; private set; }
            protected GameObject ActiveImage { get; private set; }
            protected MultiplayerWindow Window { get; private set; }

            public bool IsActive => ActiveImage.activeSelf;

            public event EventHandler OnClicked;

            public MenuItemController(MultiplayerWindow multiplayerWindow, GameObject menuItem, GameObject menuContent)
            {
                Logging.Logger.Info($"Creating {nameof(MenuItemController)}. Type={this.GetType().Name}");

                MenuItem = menuItem;
                MenuContent = menuContent;
                Window = multiplayerWindow;
            }

            public void Initialize()
            {
                if (_isInitialized)
                {
                    return;
                }

                _isInitialized = true;
                Button.OnHover.AddListener(OnHover);
                Button.OnLeftClick.AddListener(OnClickedInternal);
                ActiveImage = this.MenuItem.transform.Find(SelectedGameObjectName).gameObject;
                _hoverImage = this.MenuItem.transform.Find(HoverGameObjectName).gameObject;

                Deactivate();
            }

            private void OnHover(bool state)
            {
                _hoverImage.SetActive(state);
            }

            public virtual void Activate()
            {
                ActiveImage.SetActive(true);
                MenuContent.SetActive(true);

                Logging.Logger.Info($"Activated {nameof(MenuItemController)}. Type={this.GetType().Name}");
            }

            public virtual void Deactivate()
            {
                ActiveImage.SetActive(false);
                MenuContent.SetActive(false);

                Logging.Logger.Info($"Deactivated {nameof(MenuItemController)}. Type={this.GetType().Name}");

            }

            private void OnClickedInternal()
            {
                OnClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        public class MultiplayerWindow : FullScreenTabsWindow
        {
            private const string BaseLayoutName = "CreditsScreen";
            private const string SeparatorGameObjectName = "Separator";

            private const string MultiplayerMenuItemsObjectName = "MultiplayerMenuItems";
            private const string HostMenuItemContentObjectName = "HostMenuItemContent";
            private const string JoinMenuItemContentObjectName = "JoinMenuItemContent";
            private const string MenuOverridesObjectName = "MenuEntity_NoOverrides";

            public override FullScreenUIType ActiveFullScreenUIType => (FullScreenUIType)555555;

            private List<DOTweenAnimation> _animations = [];

            private bool _isInitialized = false;

            private MenuItemController _hostMenuController;
            private MenuItemController _joinMenuController;

            public MultiplayerWindow()
            {
                // I assume this should be used to display menu items content,
                // but I have no idea how to make it work, so have to rely on my own `MenuItemController.MenuContent` implementation
                SubWindowsList = [];
            }

            public override void Initialize()
            {
                if (_isInitialized)
                {
                    Logging.Logger.Warning("Trying to initialize already initialized window");
                    return;
                }

                SetupLayout();

                _isInitialized = true;
                base.Initialize();
                IsAnimated = true;
                var canvas = GetComponent<CanvasGroup>();
                canvas.alpha = 0f;
                _animations = [.. GetComponents<DOTweenAnimation>()];
                var closeButton = GetComponentInChildren<OwlcatButton>();
                closeButton.OnLeftClick.AddListener(OnCloseClicked);
            }

            public override void AppearAnimation()
            {
                base.AppearAnimation();
                this.gameObject.SetActive(true);
                foreach (var animation in _animations)
                {
                    animation.DOPlayForward();
                }
            }

            public override void DisappearAnimation()
            {
                base.DisappearAnimation();
                foreach (var animation in _animations)
                {
                    animation.DOPlayBackwards();
                }
                this.gameObject.SetActive(false);
            }

            public override void OnHide()
            {
                _joinMenuController.Deactivate();
                _hostMenuController.Deactivate();
                StopAllCoroutines();
                base.OnHide();
            }

            public override void Show(bool state)
            {
                Logging.Logger.Info($"Show/Hide {nameof(MultiplayerWindow)}. State={state}");
                if (state)
                {
                    _hostMenuController.Activate();
                }

                base.Show(state);
            }

            public void OnCloseClicked()
            {
                _hostMenuController?.Deactivate();
                _joinMenuController?.Deactivate();
                base.OnButtonClose();
            }

            private void SetupLayout()
            {
                var baseLayout = transform.Find(BaseLayoutName)?.gameObject;
                var (hostItemContent, joinItemContent) = SetupMenuItemsContentLayout(baseLayout);
                SetupMenuItemsLayout(baseLayout, hostItemContent, joinItemContent);
            }

            private (GameObject hostItemContent, GameObject joinItemContent) SetupMenuItemsContentLayout(GameObject baseLayout)
            {
                var hostItemContent = UnityEngine.Object.Instantiate(baseLayout, baseLayout.transform);
                hostItemContent.name = HostMenuItemContentObjectName;
                hostItemContent.CleanupAllChildren(x => true);
                SetupLoadSaveGamesLayout(hostItemContent);

                var joinItemContent = UnityEngine.Object.Instantiate(baseLayout, baseLayout.transform);
                joinItemContent.name = JoinMenuItemContentObjectName;
                joinItemContent.CleanupAllChildren(x => true);
                return (hostItemContent, joinItemContent);
            }

            private void SetupLoadSaveGamesLayout(GameObject hostItemContent)
            {
                var commonPCView = RootUIContext.Instance.m_CommonView as CommonPCView;
                // for some reason RootUIContext.Instance.m_CommonView is null after Loading Game -> Exiting to main menu
                // using cached copy which is always available on the first menu load
                var objToCopy = _cachedSaveLoadPCView ??= commonPCView?.m_SaveLoadPCView;
                SaveLoadPCView saveLoad = UnityEngine.Object.Instantiate(objToCopy, hostItemContent.transform);
                saveLoad.Initialize();
            }

            private void SetupMenuItemsLayout(GameObject baseLayout, GameObject hostItemContent, GameObject joinItemContent)
            {
                baseLayout.name = MultiplayerMenuItemsObjectName;
                baseLayout.CleanupAllChildren(
                    x => x.name != HostMenuItemContentObjectName && x.name != JoinMenuItemContentObjectName && x.name != MenuOverridesObjectName);

                var baseMenuItem = SetupBaseMenuItem(baseLayout);
                _hostMenuController = SetupMenuController(MultiplayerMenuItemType.Host, Screen.width * 0.33f, hostItemContent, baseMenuItem, baseLayout.transform);
                _joinMenuController = SetupMenuController(MultiplayerMenuItemType.Join, Screen.width * 0.66f, joinItemContent, baseMenuItem, baseLayout.transform);
                UnityEngine.Object.DestroyImmediate(baseMenuItem);

                _hostMenuController.OnClicked += OnHostMenuItemClicked;
                _joinMenuController.OnClicked += OnJoinMenuItemClicked;
            }

            private void OnHostMenuItemClicked(object sender, EventArgs e)
            {
                _hostMenuController.Activate();
                _joinMenuController.Deactivate();
            }

            private void OnJoinMenuItemClicked(object sender, EventArgs e)
            {
                _hostMenuController.Deactivate();
                _joinMenuController.Activate();
            }

            private GameObject SetupBaseMenuItem(GameObject baseLayoutObject)
            {
                var baseItem = baseLayoutObject.transform.GetChild(0).gameObject;

                UnityEngine.Object.DestroyImmediate(baseItem.GetComponent<OwlcatMultiButton>());
                baseItem.AddComponent<OwlcatButton>();

                var endSeparator = baseItem.transform.Find(SeparatorGameObjectName);
                UnityEngine.Object.DestroyImmediate(endSeparator.gameObject);

                return baseItem;
            }

            private MenuItemController SetupMenuController(
                MultiplayerMenuItemType menuItemType,
                float positionX,
                GameObject menuContent,
                GameObject baseMenuItem,
                Transform parent)
            {
                var menuItem = UnityEngine.Object.Instantiate(baseMenuItem, parent);
                var position = new Vector3(positionX, menuItem.transform.position.y, menuItem.transform.position.z);
                menuItem.transform.SetPositionAndRotation(position, menuItem.transform.rotation);

                MenuItemController controller = menuItemType switch
                {
                    MultiplayerMenuItemType.Host => new HostMenuItemController(this, menuItem, menuContent),
                    MultiplayerMenuItemType.Join => new JoinMenuItemController(this, menuItem, menuContent),
                    _ => throw new InvalidOperationException($"Unknown menuItemType. Value={menuItemType}")
                };

                controller.Initialize();

                return controller;
            }

            private enum MultiplayerMenuItemType
            {
                Host,
                Join
            }
        }
    }
}
