using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.DTOs;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Queries.GetSampledCardVisitGeoStatisticsByCountry;
// Include properties to be used as input for the query
public sealed record GetSampledCardVisitGeoStatisticsByCountryQuery(long? CardId,
                                                                   DateOnly StartDate,
                                                                   DateOnly EndDate,
                                                                   SampleType SampleType,
                                                                   bool IncludeEmptyPeriods = false) : IQuery<IEnumerable<CountryVisitStatisticsDTO>>;
