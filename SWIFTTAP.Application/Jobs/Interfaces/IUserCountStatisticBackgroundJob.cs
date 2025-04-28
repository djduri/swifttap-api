using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Jobs.Interfaces;

public interface IUserCountStatisticBackgroundJob : IBackgroundJob
{
    Task Run(CancellationToken cancellationToken = default);
}