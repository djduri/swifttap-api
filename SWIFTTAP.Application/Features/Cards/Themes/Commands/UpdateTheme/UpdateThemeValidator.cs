using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Cards.Themes.Commands.UpdateTheme;
public sealed class UpdateThemeValidator : AbstractValidator<UpdateThemeCommand>
{
    public UpdateThemeValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100);
        RuleFor(x => x.PrimaryColor).MaximumLength(25);
        RuleFor(x => x.SecondaryColor).MaximumLength(25);
        RuleFor(x => x.TextDark).MaximumLength(25);
        RuleFor(x => x.TextLight).MaximumLength(25);
        RuleFor(x => x.Background).MaximumLength(25);
        RuleFor(x => x.TextOnButtons).MaximumLength(25);
    }
}
