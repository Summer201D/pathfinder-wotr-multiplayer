using Kingmaker.Settings;
using Kingmaker.UI.MVVM._VM.Settings.Entities;
using Kingmaker.UI.SettingsUI;
using UniRx;

namespace WOTRMultiplayer.UI.Settings
{
    public abstract class SettingsEntityInputVMBase : SettingsEntityWithValueVM, ISettingsEntityInputVM
    {
        public ReactiveProperty<bool> IsValid { get; set; } = new(true);

        public ReactiveCommand<string> OnResetTempValue { get; set; } = new();

        bool ISettingsEntityInputVM.IsValid => IsValid.Value;

        public string Value
        {
            get
            {
                // this interface doesn't expose temp value for some reason
                if (m_UISettingsEntity.SettingsEntity is not SettingsEntityString stringSettting)
                {
                    var stringValue = m_UISettingsEntity.SettingsEntity.GetStringValue();
                    return stringValue;
                }

                var value = stringSettting.TempValueIsConfirmed ? stringSettting.GetValue() : stringSettting.GetTempValue();
                return value;
            }
        }

        public bool IsModificationAllowed => m_UISettingsEntity.ModificationAllowed;

        public int CharacterLimit { get; private set; }

        protected SettingsEntityInputVMBase(IUISettingsEntityWithValueBase settingEntity, int characterLimit)
            : base(settingEntity)
        {
            CharacterLimit = characterLimit;
        }

        public abstract void OnValueChanged(string value);

        public void RevertTempValue()
        {
            m_UISettingsEntity.SettingsEntity.RevertTempValue();
            IsValid.Value = true;
            OnResetTempValue.Execute(Value);
        }

        public void RevertToDefault()
        {
            base.ResetToDefault();
            OnResetTempValue.Execute(m_UISettingsEntity.SettingsEntity.GetStringDefaultValue());
        }
    }
}
