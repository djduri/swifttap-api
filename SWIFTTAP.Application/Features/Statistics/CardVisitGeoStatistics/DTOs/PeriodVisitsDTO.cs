namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.DTOs;
public class PeriodVisitsDTO
{
    public string Period { get; set; } = null!; // np. "2025-11" lub "2025-W47" albo "2025-11-20"
    public int Count { get; set; }
}
