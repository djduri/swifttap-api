using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Specifications;
internal sealed class FindVcdDownloadStatisticByCardIdAndDateSpecification : Specification<VcfDownloadStatistic>
{
    public FindVcdDownloadStatisticByCardIdAndDateSpecification(long cardId, DateOnly dateOnly)
        : base(x => x.CardId == cardId && x.Date == dateOnly)
    {
    }
}
