using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Services.Interfaces;

internal interface ITokenService : IScopedAppService
{
    Task<TokenDTO> CreateTokenWithRefreshTokenAsync(User user, CancellationToken cancellationToken);
    Task<long> ValidateRefreshTokenAndGetUserIdAsync(string refreshTokenValue, CancellationToken cancellationToken);
}