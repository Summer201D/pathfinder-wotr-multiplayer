using System;
using System.Collections.Generic;
using DG.Tweening;
using Kingmaker.UI.FullScreenUITypes;
using Kingmaker.UI.MVVM;
using Kingmaker.UI.MVVM._PCView.Common;
using Kingmaker.UI.MVVM._PCView.SaveLoad;
using Kingmaker.UI.ServiceWindow;
using Owlcat.Runtime.UI.Controls.Button;
using UnityEngine;
using WOTRMultiplayer.Extensions;
using WOTRMultiplayer.Menu.Items;

namespace WOTRMultiplayer.Menu
{
    public class MultiplayerWindow : FullScreenTabsWindow
    {
        private static SaveLoadPCView _cachedSaveLoadPCView;

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
