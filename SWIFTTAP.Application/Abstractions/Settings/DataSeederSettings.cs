using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class DataSeederSettings : IValidatableSettings
{
	public AdminUser? AdminUser { get; init; }

    public bool Valid()
    {
		if (AdminUser is null) return false;

		if (string.IsNullOrWhiteSpace(AdminUser.Email)) return false;

        if (string.IsNullOrWhiteSpace(AdminUser.Password)) return false;

		return true;
    }
}

public sealed class AdminUser
{
	public string? Email { get; init; }
	public string? Password { get; init; }
	public bool EmailConfirmed { get; init; }
	public bool PhoneNumberConfirmed { get; init; }
}
