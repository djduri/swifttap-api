using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Application.Services;

internal sealed class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataSeederSettings _dataSeederSettings;

    public UserService(UserManager<User> userManager,
                       IHttpContextAccessor httpContextAccessor,
                       IOptions<DataSeederSettings> dataSeederSettings)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _dataSeederSettings = dataSeederSettings.Value;
    }

    public async Task<User> GetAuthenticatedUser()
    {
        return await GetAuthenticatedUserOrDefaultAsync() ??
               throw AuthorizationException.FromErrorCode(ErrorCodes.User.NotAuthenticated);
    }

    public async Task<User?> GetAuthenticatedUserOrDefaultAsync()
    {
        var userId = GetUserIdFromClaims();
        return userId is null ? null : await _userManager.FindByIdAsync(userId);
    }

    public long GetAuthenticatedUserId()
    {
        return GetAuthenticatedUserIdOrDefault() ??
               throw AuthorizationException.FromErrorCode(ErrorCodes.User.NotAuthenticated);
    }

    public long? GetAuthenticatedUserIdOrDefault()
    {
        var userId = GetUserIdFromClaims();
        return userId is null ? null : long.Parse(userId);
    }

    public long GetAuthenticatedUserCardId()
    {
        return GetAuthenticatedUserCardIdOrDefault() ??
               throw AuthorizationException.FromErrorCode(ErrorCodes.User.NotAuthenticated);
    }

    public long? GetAuthenticatedUserCardIdOrDefault()
    {
        var cardId = GetUserCardIdFromClaims();
        return cardId is null ? null : long.Parse(cardId);
    }

    public string? GetAuthenticatedUserEmailOrDefault()
    {
        return GetUserEmailFromClaims();
    }

    public async Task<bool> IsUserInRoleAsync(long userId, Roles role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                   throw EntityNotFoundException.FromErrorCode(ErrorCodes.User.NotFound);

        cancellationToken.ThrowIfCancellationRequested();

        return await _userManager.IsInRoleAsync(user, role.ToString());
    }

    public bool IsUserSuperAdmin(User user) => user.Email == _dataSeederSettings.AdminUser.Email;

    public bool IsAuthenticatedUserAdmin() => HasAuthenticatedUserRole(Roles.Admin);

    public bool IsAuthenticatedUserNotAdmin() => !HasAuthenticatedUserRole(Roles.Admin);

    public bool HasAuthenticatedUserRole(params Roles[] roles)
    {
        var user = _httpContextAccessor.HttpContext?.User ??
                   throw AuthorizationException.FromErrorCode(ErrorCodes.User.NotAuthenticated);

        var userRoles = user.Claims
                            .Where(x => x.Type == ClaimTypes.Role)
                            .Select(x => x.Value);

        return roles.All(role => userRoles.Contains(role.ToString()));
    }

    public bool HasAuthUserPermissionToUser(long targetUserId)
    {   
        // Admin może wszystko
        if (IsAuthenticatedUserAdmin())
            return true;

        // Zwykły użytkownik może tylko siebie
        var authUserId = GetAuthenticatedUserId();
        return authUserId == targetUserId;
    }

    public bool HasAuthUserPermissionToCard(long cardId)
    {
        // Admin może wszystko
        if (IsAuthenticatedUserAdmin())
            return true;

        var authUserCardId = GetAuthenticatedUserCardId();

        // Zwykły użytkownik może tylko siebie
        return authUserCardId == cardId;
    }

    // Prywatne metody
    private string? GetUserIdFromClaims() => GetClaimValue(ClaimTypes.NameIdentifier);

    private string? GetUserCardIdFromClaims() => GetClaimValue("CardId");

    private string? GetUserEmailFromClaims() => GetClaimValue(ClaimTypes.Email);

    private string? GetClaimValue(string claimType)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
    }
}
