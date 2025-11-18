using FluentValidation;
using Kingmaker.UI.SettingsUI;

namespace WOTRMultiplayer.UI.Settings.Entities
{
    public abstract class UIValidatableSettingsEntityBase<TValue> : UISettingsEntityWithValueBase<TValue>
    {
        public AbstractValidator<TValue> Validator { get; set; }

        public int CharacterLimit { get; set; }

        public override SettingsListItemType? Type => SettingsListItemType.Custom;
    }
}
