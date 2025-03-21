using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Authorization.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Authorization;
internal sealed class AuthorizationSeederService : IAuthorizationSeederService
{
    private readonly RoleManager<IdentityRole<long>> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly DataSeederSettings _dataSeederSettings;
    private readonly DatabaseContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorizationSeederService(RoleManager<IdentityRole<long>> roleManager,
                                      UserManager<User> userManager,
                                      IOptions<DataSeederSettings> dataSeederSeetings,
                                      DatabaseContext dbContext,
                                      IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _dataSeederSettings = dataSeederSeetings.Value;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }
    public async Task SeedAsync()
    {
        // Seed permissions and roles
        await SeedRolesAndPermissions();

        // Update the super admin permissions if needed
        await UpdateSuperAdminPermissions();
    }

    private async Task SeedRolesAndPermissions()
    {
        // Get all defined roles from the enumeration
        var allRoles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();

        // Fetch all existing roles from the database
        var existingRoles = await _roleManager.Roles.ToListAsync();

        // Find roles to delete (existing roles not defined in the enum)
        var rolesToDelete = existingRoles.Where(role => !allRoles.Any(r => r.ToString() == role.Name)).ToList();

        if (rolesToDelete.Any())
        {
            // Remove roles that are not in the enum
            _dbContext.RemoveRange(rolesToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        // Re-fetch the updated list of existing roles
        existingRoles = await _roleManager.Roles.ToListAsync();

        // Ensure all roles defined in the enum exist in the database
        foreach (var roleEnum in allRoles)
        {
            var roleName = roleEnum.ToString();

            // Only create the role if it does not exist
            if (!existingRoles.Any(role => role.Name == roleName))
            {
                var newRole = new IdentityRole<long>(roleName);
                await _roleManager.CreateAsync(newRole);
            }
        }
    }

    private async Task UpdateSuperAdminPermissions()
    {
        // Check if an email for the admin user is provided
        if (string.IsNullOrWhiteSpace(_dataSeederSettings.AdminUser!.Email)) return;

        // Find the user by email
        var adminUser = await _userManager.FindByEmailAsync(_dataSeederSettings.AdminUser.Email);
        if (adminUser is null) return;

        // Fetch the user's current roles
        var currentRoles = await _userManager.GetRolesAsync(adminUser);

        // If the user already has only the Admin role, no update is needed
        if (currentRoles.Count == 1 && currentRoles.Contains(Roles.Admin.ToString())) return;

        // Remove all existing roles
        foreach (var role in currentRoles)
        {
            await _userManager.RemoveFromRoleAsync(adminUser, role);
        }

        // Assign the Admin role
        await _userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
    }
}

