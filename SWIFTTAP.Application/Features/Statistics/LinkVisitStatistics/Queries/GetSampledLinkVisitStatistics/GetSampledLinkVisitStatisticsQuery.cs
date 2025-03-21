using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Queries.GetSampledLinkVisitStatistics;
// Include properties to be used as input for the query
public sealed record GetSampledLinkVisitStatisticsQuery(long LinkId,
                                                        DateOnly StartDate,
                                                        DateOnly EndDate,
                                                        int NumberOfSamples) : IQuery<IEnumerable<VisitStatisticSampledDTO>>;
