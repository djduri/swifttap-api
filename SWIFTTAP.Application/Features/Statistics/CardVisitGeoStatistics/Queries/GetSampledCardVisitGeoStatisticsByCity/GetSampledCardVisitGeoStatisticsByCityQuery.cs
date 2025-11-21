using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.DTOs;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Queries.GetSampledCardVisitGeoStatisticsByCity;

// Include properties to be used as input for the query
public sealed record GetSampledCardVisitGeoStatisticsByCityQuery(long? CardId,
                                                           DateOnly StartDate,
                                                           DateOnly EndDate,
                                                           SampleType SampleType,
                                                           bool IncludeEmptyPeriods = false) : IQuery<IEnumerable<CityVisitStatisticsDTO>>;



