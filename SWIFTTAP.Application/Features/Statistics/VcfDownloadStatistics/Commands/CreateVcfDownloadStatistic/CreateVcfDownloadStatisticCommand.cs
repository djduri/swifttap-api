using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Commands.CreateVcfDownloadStatistic;
// Include properties to be used as input for the command
public sealed record CreateVcfDownloadStatisticCommand(string UniqueName) : ICommand<long>;