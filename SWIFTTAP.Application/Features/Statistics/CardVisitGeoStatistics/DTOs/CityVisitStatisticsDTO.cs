namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.DTOs;
public class CityVisitStatisticsDTO
{
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public List<PeriodVisitsDTO> Visits { get; set; } = new();
}



