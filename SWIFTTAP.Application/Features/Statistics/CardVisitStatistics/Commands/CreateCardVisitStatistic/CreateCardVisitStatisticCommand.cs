using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Commands.CreateCardVisitStatistic;
// Include properties to be used as input for the command
public sealed record CreateCardVisitStatisticCommand(long CardId) : ICommand<long>;