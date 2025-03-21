using Microsoft.AspNetCore.Identity;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Extensions;

internal static class UserManagerExtensions
{
    public static async Task<bool> UserExistsByEmailAsync(this UserManager<User> @this, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        var user = await @this.FindByEmailAsync(email);

        return user != null;
    }
}
