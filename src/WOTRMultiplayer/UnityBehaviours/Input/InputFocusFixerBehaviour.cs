using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WOTRMultiplayer.UnityBehaviours.Input
{
    /// <summary>
    /// Esc stops to work when you press ESC while input is focused. GameObject remains selected and eats Esc input, so you can't press ESC again to close current window
    /// This happens in multiple places:
    ///     - vanilla game (Credits -> Search)
    ///     - Multiplayer settings
    ///     - Join Lobby tab
    /// so we could either handle ESC or select the parent object when the input is no longer focused. This behavior uses the latter approach and applies to every modded input.
    public class InputFocusFixerBehaviour : MonoBehaviour
    {
        private TMP_InputField _input;

        private void Awake()
        {
            _input = this.GetComponent<TMP_InputField>();
        }

        private void Update()
        {
            if (_input != null && !_input.isFocused && this.transform.parent != null && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(this.transform.parent.gameObject);
            }
        }
    }
}
