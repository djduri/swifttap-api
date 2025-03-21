using FluentValidation;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.UpdateTranslation;

public sealed class UpdateTranslationValidator : AbstractValidator<UpdateTranslationCommand>
{
    public UpdateTranslationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Language).NotNull().IsInEnum();
    }
}
