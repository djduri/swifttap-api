using SWIFTTAP.Domain.Core;

namespace SWIFTTAP.Domain.Administration;
public sealed class UserKeys
{
    public long UserId { get; set; }
    public User User { get; set; }

    public string? AuthTwoFactorKey { get; private set; }
    public string? AuthTwoFactorCode { get; private set; }
    public DateTime? AuthTwoFactorCodeValidUntil { get; private set; }

    public string? PreAuthenticationToken { get; private set; }
    public DateTime? PreAuthenticationTokenValidUntil { get; private set; }

    public void SetPreAuthenticationToken()
    {
        PreAuthenticationTokenValidUntil = DateTime.UtcNow.AddMinutes(15);
    }
    public bool IsPreAuthenticationTokenStillValid()
    {
        return PreAuthenticationTokenValidUntil > DateTime.UtcNow;
    }
    public void SetAuthTwoFactorKey(int twoFactorCodeExpirationInMinutes)
    {
        AuthTwoFactorKey = SecretBuilder.GenerateGuidToken(8);
        AuthTwoFactorCodeValidUntil = DateTime.UtcNow.AddMinutes(twoFactorCodeExpirationInMinutes);
        AuthTwoFactorCode = SecretBuilder.GenerateUserFriendlyCode(6);
    }

    public bool IsAuthTwoFactorCodeStillValid()
    {
        return AuthTwoFactorCodeValidUntil >= DateTime.UtcNow;
    }

    public void ResetAuthTwoFactorKey()
    {
        AuthTwoFactorKey = null;
        AuthTwoFactorCodeValidUntil = null;
        AuthTwoFactorCode = null;
    }
}
