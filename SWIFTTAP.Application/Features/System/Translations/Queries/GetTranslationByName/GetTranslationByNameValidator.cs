using FluentValidation;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetTranslationByName;

public sealed class GetTranslationByNameValidator : AbstractValidator<GetTranslationByNameQuery>
{
    public GetTranslationByNameValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
