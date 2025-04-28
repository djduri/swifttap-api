using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Statistics.UserCountStatistics.DTOs;

namespace SWIFTTAP.Application.Features.Statistics.UserCountStatistics.Queries.GetSampledUserCountStatistics;
// Include properties to be used as input for the query
public sealed record GetSampledUserCountStatisticsQuery(DateOnly StartDate,
                                                        DateOnly EndDate,
                                                        int NumberOfSamples) : IQuery<IEnumerable<UserCountStatisticSampleDTO>>;
