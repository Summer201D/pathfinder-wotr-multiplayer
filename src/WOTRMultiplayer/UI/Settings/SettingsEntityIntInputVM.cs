using WOTRMultiplayer.UI.Settings.Entities;

namespace WOTRMultiplayer.UI.Settings
{
    public class SettingsEntityIntInputVM : SettingsEntityInputVMBase
    {
        private readonly UIValidatableSettingsEntityBase<int> _settingEntity;

        public SettingsEntityIntInputVM(UIValidatableSettingsEntityBase<int> settingEntity)
            : base(settingEntity, settingEntity.CharacterLimit)
        {
            _settingEntity = settingEntity;
        }

        public override void OnValueChanged(string value)
        {
            if (int.TryParse(value?.Trim(), out var intValue))
            {
                _settingEntity.SetTempValue(intValue);
            }

            if (_settingEntity.Validator != null)
            {
                var validation = _settingEntity.Validator.Validate(intValue);
                IsValid.Value = validation.IsValid;
            }
        }
    }
}
