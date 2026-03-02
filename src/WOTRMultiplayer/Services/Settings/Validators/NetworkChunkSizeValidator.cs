using FluentValidation;

namespace WOTRMultiplayer.Services.Settings.Validators
{
    public class NetworkChunkSizeValidator : AbstractValidator<int>
    {
        public const int MaxLength = 10;

        public NetworkChunkSizeValidator()
        {
            RuleFor(x => x).GreaterThanOrEqualTo(1);
            RuleFor(x => x).LessThanOrEqualTo(int.MaxValue);
        }
    }
}
