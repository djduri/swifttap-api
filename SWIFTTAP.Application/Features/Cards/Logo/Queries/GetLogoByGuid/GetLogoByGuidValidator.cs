using FluentValidation;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByGuid;
public sealed class GetLogoByGuidValidator : AbstractValidator<GetLogoByGuidQuery>
{
    public GetLogoByGuidValidator()
    {
        RuleFor(x => x.Guid).NotEmpty();
    }
}
