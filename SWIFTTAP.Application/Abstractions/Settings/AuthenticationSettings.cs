using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class AuthenticationSettings : IValidatableSettings
{
	public bool IsDevelopment { get; init; }
	public string? JwtSecret { get; init; }
    public int RefreshTokenExpirationDays { get; init; }
    public int TokenExpirationMinutes { get; init; }
    public int TwoFactorCodeExpirationMinutes { get; init; }
    public IReadOnlyCollection<string> Issuers { get; init; } = [];
    public IReadOnlyCollection<string> Audiences { get; init; } = [];

    public bool Valid()
    {
        if (string.IsNullOrEmpty(JwtSecret)) return false;  
        if (RefreshTokenExpirationDays <= 0) return false;  
        if (TokenExpirationMinutes <= 0) return false;  
        if (TwoFactorCodeExpirationMinutes <= 0) return false;  
        if (Issuers is null) return false;        
        if (Audiences is null) return false;

        return true;
    }
}
