using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Specifications;
internal sealed class FindCardVisitStatisticByCardIdAndDateSpecification : Specification<CardVisitStatistic>
{
    public FindCardVisitStatisticByCardIdAndDateSpecification(long cardId, DateOnly dateOnly)
        : base(x => x.CardId == cardId && x.Date == dateOnly)
    {
    }
}
