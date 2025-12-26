using System;
using UnityEngine;
using WOTRMultiplayer.UI;

namespace WOTRMultiplayer.Abstractions.UI.Controllers.Menu
{
    public interface IMultiplayerMenuItemController : IDisposable
    {
        ModalActionConfirmation GetDeactivationConfirmation();

        void Initialize(GameObject baseLayout, GameObject menuItem);

        void Activate();

        void Deactivate();

        bool IsActive { get; }

        Action<object, EventArgs> OnClicked { get; set; }

        Action<bool> OnChangeWindowVisibility { get; set; }
    }
}
