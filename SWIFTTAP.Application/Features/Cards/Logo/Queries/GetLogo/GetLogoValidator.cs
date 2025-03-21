using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogo;
public sealed class GetLogoValidator : AbstractValidator<GetLogoQuery>
{
    public GetLogoValidator()
    {
        RuleFor(x => x.CardId).IsIdentifier();
    }
}
