using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Authorization.Interfaces;

internal interface IAuthorizationSeederService : IScopedAppService
{
    Task SeedAsync();
}