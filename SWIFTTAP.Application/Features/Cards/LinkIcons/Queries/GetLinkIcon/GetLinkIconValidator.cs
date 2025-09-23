using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.LinkIcons.Queries.GetLinkIcon;

public sealed class GetLinkIconValidator : AbstractValidator<GetLinkIconQuery>
{
    public GetLinkIconValidator()
    {
        RuleFor(x => x.LinkId).IsIdentifier();
    }
}
