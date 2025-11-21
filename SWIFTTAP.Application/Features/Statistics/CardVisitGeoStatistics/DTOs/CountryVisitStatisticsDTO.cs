namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.DTOs;
public class CountryVisitStatisticsDTO
{
    public required string Country { get; set; }
    public List<PeriodVisitsDTO> Visits { get; set; } = new();
}
