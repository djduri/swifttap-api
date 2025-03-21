namespace SWIFTTAP.Application.Abstractions;

public interface IBackgroundJob
{
    Task Run(CancellationToken cancellationToken = default);
}
