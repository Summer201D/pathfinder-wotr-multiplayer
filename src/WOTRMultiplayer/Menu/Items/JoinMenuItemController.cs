using TMPro;
using UnityEngine;
using WOTRMultiplayer.Strings;

namespace WOTRMultiplayer.Menu.Items
{
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
}
