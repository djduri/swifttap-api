using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Specifications;
internal class FindLinkVisitStatisticByLinkIdAndDateSpecification : Specification<LinkVisitStatistic>
{
    public FindLinkVisitStatisticByLinkIdAndDateSpecification(long linkId, DateOnly dateOnly)
        : base(x => x.LinkId == linkId && x.Date == dateOnly)
    {
    }
}
