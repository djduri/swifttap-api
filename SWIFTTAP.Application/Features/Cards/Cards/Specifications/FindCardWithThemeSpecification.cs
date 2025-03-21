using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Specifications;
internal sealed class FindCardWithThemeSpecification : Specification<Card>
{
    public FindCardWithThemeSpecification(long id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.Theme);
    }
}
