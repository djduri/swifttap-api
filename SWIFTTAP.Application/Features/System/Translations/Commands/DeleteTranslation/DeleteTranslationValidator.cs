using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.DeleteTranslation;
public sealed class DeleteTranslationValidator : AbstractValidator<DeleteTranslationCommand>
{
    public DeleteTranslationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
