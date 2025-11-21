using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Commands.CreateCardVisitGeoStatistic
{
    // Include properties to be used as input for the command
    public sealed record CreateCardVisitGeoStatisticCommand(long CardId) : ICommand<long>;
}