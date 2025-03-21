using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Services.Interfaces;
public interface IStatisticsService : IScopedAppService
{
    IList<VisitStatisticSampledDTO> ApplySampling(IEnumerable<VisitStatistic> visitStatistics, DateOnly startDate, DateOnly endDate, int samplesCount);
}