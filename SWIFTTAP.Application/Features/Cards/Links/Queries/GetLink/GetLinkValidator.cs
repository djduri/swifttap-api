using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.Links.Queries.GetLink;
public sealed class GetLinkValidator : AbstractValidator<GetLinkQuery>
{
    public GetLinkValidator()
    {
        RuleFor(x => x.Id).IsIdentifier();
    }
}
