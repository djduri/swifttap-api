namespace SWIFTTAP.Application.Common.DTOs;
public class VisitStatisticSampledDTO
{
    public required DateOnly Date { get; set; }
    public required int TotalVisits { get; set; }
    public required int AverageVisits { get; set; }
}
