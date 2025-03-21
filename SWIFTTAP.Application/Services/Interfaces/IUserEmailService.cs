using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Services.Interfaces;

internal interface IUserEmailService : IScopedAppService
{
    Task<bool> Send2FaAuthenticationCodeAsync(User user);
}