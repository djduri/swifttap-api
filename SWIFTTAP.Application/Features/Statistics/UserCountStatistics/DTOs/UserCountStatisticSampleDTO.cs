namespace SWIFTTAP.Application.Features.Statistics.UserCountStatistics.DTOs;
public sealed class UserCountStatisticSampleDTO
{
    public required DateOnly Date { get; set; }
    public required int TotalUsers { get; set; }
    public required int AverageUsers { get; set; }
}
