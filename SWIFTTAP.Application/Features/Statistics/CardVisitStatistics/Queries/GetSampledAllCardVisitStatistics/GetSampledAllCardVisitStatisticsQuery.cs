using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Queries.GetSampledAllCardVisitStatistics;
// Include properties to be used as input for the query
public sealed record GetSampledAllCardVisitStatisticsQuery(DateOnly StartDate,
                                                           DateOnly EndDate,
                                                           int NumberOfSamples) : IQuery<IEnumerable<VisitStatisticSampledDTO>>;
