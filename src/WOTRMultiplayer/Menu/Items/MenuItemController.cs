using System;
using Owlcat.Runtime.UI.Controls.Button;
using UnityEngine;

namespace WOTRMultiplayer.Menu.Items
{
    public abstract class MenuItemController
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
}
