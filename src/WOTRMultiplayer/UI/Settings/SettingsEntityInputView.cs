using Kingmaker.UI.MVVM._PCView.Settings.Entities;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace WOTRMultiplayer.UI.Settings
{
    public class SettingsEntityInputView : SettingsEntityWithValueView<SettingsEntityInputVMBase>
    {
        private GameObject _inputObject;
        private Color _invalidFormatColor = Color.red;
        private Color _validFormatColor;

        private TMP_InputField InputField => _inputObject.GetComponent<TMP_InputField>();

        public override void BindViewImplementation()
        {
            base.BindViewImplementation();

            // restoring references since everything is detached
            m_HighlightedImage = this.gameObject.transform.Find("HighlightedImage").GetComponent<Image>();
            m_Title = this.gameObject.transform.Find("HorizontalLayoutGroup/Text").GetComponent<TextMeshProUGUI>();
            m_MarkImage = this.gameObject.transform.Find("HorizontalLayoutGroup/PointGroup/MarkImage").GetComponent<Image>();
            m_PointImage = this.gameObject.transform.Find("HorizontalLayoutGroup/PointGroup/PointImage").GetComponent<Image>();

            _inputObject = this.gameObject.transform.Find("MultiButton").GetChild(0).gameObject;

            m_Title.text = ViewModel.Title;
            _validFormatColor = m_Title.color;

            AddDisposable(ViewModel.IsValid.Subscribe(OnIsValidChanged));
            AddDisposable(ViewModel.OnResetTempValue.Subscribe(OnResetTempValue));


            var placeholder = _inputObject.transform.Find(UIFactory.InputPlaceholderObjectName);
            var placeholderText = placeholder.GetComponent<TextMeshProUGUI>();
            placeholderText.SetText(string.Empty);
            placeholderText.alignment = TextAlignmentOptions.Center;

            var input = _inputObject.transform.Find(UIFactory.InputLabelObjectName);
            var inputText = input.GetComponent<TextMeshProUGUI>();
            inputText.overflowMode = TextOverflowModes.Truncate;
            inputText.alignment = TextAlignmentOptions.Center;

            InputField.text = ViewModel.Value;
            InputField.characterLimit = ViewModel.CharacterLimit;
            InputField.onValueChanged.AddListener(ViewModel.OnValueChanged);

            if (!ViewModel.IsModificationAllowed)
            {
                inputText.color = Color.grey;
                InputField.interactable = false;
            }
        }

        public override void DestroyViewImplementation()
        {
            InputField.onValueChanged.RemoveAllListeners();

            ViewModel.IsValid?.Dispose();
            ViewModel.OnResetTempValue?.Dispose();
            ViewModel.Dispose();
            ViewModel = null;
        }

        public override void OnModificationChanged(bool allowed)
        {
            ViewModel.ModificationAllowed.Value = allowed;
        }

        private void OnResetTempValue(string newValue)
        {
            InputField.text = newValue;
        }

        private void OnIsValidChanged(bool isValid)
        {
            m_Title.color = isValid ? _validFormatColor : _invalidFormatColor;
        }
    }
}
