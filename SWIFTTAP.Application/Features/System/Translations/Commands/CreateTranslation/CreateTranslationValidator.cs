using FluentValidation;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.CreateTranslation;

public sealed class CreateTranslationValidator : AbstractValidator<CreateTranslationCommand>
{
    public CreateTranslationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Language).NotNull().IsInEnum();
    }
}
