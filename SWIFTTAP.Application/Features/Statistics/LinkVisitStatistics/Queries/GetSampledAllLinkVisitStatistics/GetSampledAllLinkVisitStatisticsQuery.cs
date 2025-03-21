using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Queries.GetSampledAllLinkVisitStatistics;
// Include properties to be used as input for the query
public sealed record GetSampledAllLinkVisitStatisticsQuery(DateOnly StartDate,
                                                           DateOnly EndDate,
                                                           int NumberOfSamples) : IQuery<IEnumerable<VisitStatisticSampledDTO>>;
