using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Specifications;
internal sealed class FindLinkByCardIdAndOrderSpecification : Specification<Link>
{
    public FindLinkByCardIdAndOrderSpecification(long cardId, int order)
        : base(x => x.CardId == cardId && x.Order == order)
    {
    }
}
