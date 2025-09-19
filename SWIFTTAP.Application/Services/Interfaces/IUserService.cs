using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Services.Interfaces;

public interface IUserService : IScopedAppService
{
    Task<User> GetAuthenticatedUser();
    Task<User?> GetAuthenticatedUserOrDefaultAsync();
    long GetAuthenticatedUserId();
    long? GetAuthenticatedUserIdOrDefault();
    long GetAuthenticatedUserCardId();
    long? GetAuthenticatedUserCardIdOrDefault();
    string? GetAuthenticatedUserEmailOrDefault();
    Task<bool> IsUserInRoleAsync(long userId, Roles role, CancellationToken cancellationToken = default);
    bool IsAuthenticatedUserAdmin();
    bool IsAuthenticatedUserNotAdmin();
    bool IsUserSuperAdmin(User user);
    bool HasAuthenticatedUserRole(params Roles[] roles);
    bool HasAuthUserPermissionToUser(long targetUserId);
    bool HasAuthUserPermissionToCard(long cardId);
}