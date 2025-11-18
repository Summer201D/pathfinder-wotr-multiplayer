using WOTRMultiplayer.UI.Settings.Entities;

namespace WOTRMultiplayer.UI.Settings
{
    public class SettingsEntityStringInputVM : SettingsEntityInputVMBase
    {
        private readonly UIValidatableSettingsEntityBase<string> _settingEntity;

        public SettingsEntityStringInputVM(UIValidatableSettingsEntityBase<string> settingEntity)
            : base(settingEntity, settingEntity.CharacterLimit)
        {
            _settingEntity = settingEntity;
        }

        public override void OnValueChanged(string value)
        {
            _settingEntity.SetTempValue(value);
            if (_settingEntity.Validator != null)
            {
                var validation = _settingEntity.Validator.Validate(value);
                IsValid.Value = validation.IsValid;
            }
        }
    }
}
