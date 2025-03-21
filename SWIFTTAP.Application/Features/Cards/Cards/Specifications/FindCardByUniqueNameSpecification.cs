using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Specifications;
internal sealed class FindCardByUniqueNameSpecification : Specification<Card>
{
    public FindCardByUniqueNameSpecification(string uniqueName)
        : base(x => x.UniqueName == uniqueName)
    {
    }
}
