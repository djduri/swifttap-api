using FluentValidation;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByUniqueName;
public sealed class GetLogoByUniqueNameValidator : AbstractValidator<GetLogoByUniqueNameQuery>
{
    public GetLogoByUniqueNameValidator()
    {
        RuleFor(x => x.UniqueName).NotEmpty();
    }
}
