using FluentValidation;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetTranslation;

public sealed class GetTranslationValidator : AbstractValidator<GetTranslationQuery>
{
    public GetTranslationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
