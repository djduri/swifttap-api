using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Core;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SWIFTTAP.Application.Services;
internal sealed class TokenService : ITokenService
{
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TokenService> _logger;
    private readonly DatabaseContext _databaseContext;

    public TokenService(IOptions<AuthenticationSettings> authenticationSettings,
                        UserManager<User> userManager,
                        IUnitOfWork unitOfWork,
                        ILogger<TokenService> logger,
                        DatabaseContext databaseContext)
    {
        _authenticationSettings = authenticationSettings.Value;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public async Task<TokenDTO> CreateTokenWithRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var refreshToken = await GenerateRefreshTokenAsync(user.Id, cancellationToken);
        var tokenDTO = await BuildAccessTokenAsync(user, refreshToken, cancellationToken);
        return tokenDTO;
    }

    public async Task<long> ValidateRefreshTokenAndGetUserIdAsync(string refreshTokenValue, CancellationToken cancellationToken)
    {
        var refreshToken = await _databaseContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Value == refreshTokenValue, cancellationToken)
            ?? throw AuthenticationException.FromErrorCode(ErrorCodes.Authentication.Failed);

        if (refreshToken.RevokedAt.HasValue)
        {
            _logger.LogWarning("The refresh token for user ID {UserId} has been revoked. Refresh Token: {RefreshToken}", refreshToken.UserId, refreshTokenValue);
            throw AuthenticationException.FromErrorCode(ErrorCodes.Authentication.Failed);
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("The refresh token for user ID {UserId} has expired. Refresh Token: {RefreshToken}", refreshToken.UserId, refreshTokenValue);
            throw AuthenticationException.FromErrorCode(ErrorCodes.Authentication.Failed);
        }

        return refreshToken.UserId;
    }

    private async Task<TokenDTO> BuildAccessTokenAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authenticationSettings.JwtSecret!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("Name", user.Name),
            new Claim("CardId", user.Card.Id.ToString()),
            new Claim("2FA", user.TwoFactorEnabled.ToString()),
            new Claim("EmailConfirmed", user.EmailConfirmed.ToString())
        };

        claims.AddRange(await GetClaimsFromRolesAsync(user));

        var claimsIdentity = new ClaimsIdentity(new GenericIdentity(user.UserName!, "Token"), claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(_authenticationSettings.TokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new TokenDTO
        {
            Token = tokenHandler.WriteToken(token),
            TokenExpiration = tokenDescriptor.Expires.Value,
            RefreshToken = refreshToken.Value
        };
    }

    private async Task<IEnumerable<Claim>> GetClaimsFromRolesAsync(User user)
    {
        var claims = new List<Claim>();
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            if (Enum.TryParse<Roles>(role, out var roleEnum))
                claims.Add(new Claim(ClaimTypes.Role, roleEnum.ToString()));
            else
                _logger.LogWarning("Invalid role '{Role}' for user ID {UserId}. Could not map to Roles enum.", role, user.Id);
        }

        return claims;
    }

    private async Task<RefreshToken> GenerateRefreshTokenAsync(long userId, CancellationToken cancellationToken)
    {
        string refreshTokenValue = await GenerateUniqueTokenValueAsync(cancellationToken);

        var expiresAt = DateTime.UtcNow.AddDays(_authenticationSettings.RefreshTokenExpirationDays);
        var newRefreshToken = RefreshToken.Factory.Create(refreshTokenValue, userId, expiresAt);

        _databaseContext.RefreshTokens.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newRefreshToken;
    }

    private async Task<string> GenerateUniqueTokenValueAsync(CancellationToken cancellationToken)
    {
        string refreshTokenValue;
        do
        {
            refreshTokenValue = SecretBuilder.GenerateToken(512);
        } while (await _databaseContext.RefreshTokens.AnyAsync(x => x.Value == refreshTokenValue, cancellationToken));

        return refreshTokenValue;
    }
}

