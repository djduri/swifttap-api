using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetAllTranslations;
public sealed class GetAllTranslationsValidator : AbstractValidator<GetAllTranslationsQuery>
{
    public GetAllTranslationsValidator()
    {
        When(x => x.SortingArguments is not null, () =>
        {
            RuleFor(x => x.SortingArguments!).SetValidator(new SortingArgumentsValidator());
        });
        When(x => x.PaginationArguments is not null, () =>
        {
            RuleFor(x => x.PaginationArguments!).SetValidator(new PaginationArgumentsValidator());
        });
    }
}
