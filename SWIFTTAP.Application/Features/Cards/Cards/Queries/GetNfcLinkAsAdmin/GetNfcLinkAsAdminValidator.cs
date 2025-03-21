using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLinkAsAdmin;
public sealed class GetNfcLinkAsAdminValidator : AbstractValidator<GetNfcLinkAsAdminQuery>
{
    public GetNfcLinkAsAdminValidator()
    {
        RuleFor(x => x.CardId).IsIdentifier();
    }
}
