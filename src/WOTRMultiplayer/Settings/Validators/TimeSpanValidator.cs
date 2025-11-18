using System;
using FluentValidation;

namespace WOTRMultiplayer.Settings.Validators
{
    public class TimeSpanValidator : AbstractValidator<string>
    {
        public TimeSpanValidator()
        {
            RuleFor(x => x).Must(x => TimeSpan.TryParse(x, out _));
        }
    }
}
