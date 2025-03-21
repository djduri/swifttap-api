using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Specifications;
internal sealed class FindCardWithLogoSpecification : Specification<Card>
{
    public FindCardWithLogoSpecification(long id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.Logo);
    }
}
