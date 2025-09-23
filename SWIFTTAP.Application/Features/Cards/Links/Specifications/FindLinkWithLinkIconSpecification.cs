using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Specifications;

internal sealed class FindLinkWithLinkIconSpecification : Specification<Link>
{
    public FindLinkWithLinkIconSpecification(long id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.LinkIcon);
    }
}
