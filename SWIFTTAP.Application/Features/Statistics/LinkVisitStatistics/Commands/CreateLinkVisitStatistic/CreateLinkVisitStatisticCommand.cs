using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Commands.CreateLinkVisitStatistic;
// Include properties to be used as input for the command
public sealed record CreateLinkVisitStatisticCommand(long LinkId) : ICommand<long>;