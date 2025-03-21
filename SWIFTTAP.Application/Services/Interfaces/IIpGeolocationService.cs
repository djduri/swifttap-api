using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Services.Interfaces;

internal interface IIpGeolocationService : IScopedAppService
{
    Task<IpGeolocationResult?> GetGeolocationAsync(string ipAddress, CancellationToken cancellationToken = default);
}
