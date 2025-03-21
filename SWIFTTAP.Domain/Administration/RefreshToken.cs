using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Administration;
public sealed class RefreshToken
{
    public string Value { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public RefreshToken SetValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw DomainException.FromErrorCode(ErrorCodes.RefreshToken.InvalidValue);

        Value = value;
        return this;
    }

    public RefreshToken SetExpiresAt(DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            throw DomainException.FromErrorCode(ErrorCodes.RefreshToken.InvalidExpiresAt);

        ExpiresAt = expiresAt;
        return this;
    }

    public RefreshToken SetUserId(long userId)
    {
        if (userId == default)
            throw DomainException.FromErrorCode(ErrorCodes.RefreshToken.InvalidUserId);

        UserId = userId;
        return this;
    }

    public RefreshToken Revoke()
    {
        RevokedAt = DateTime.UtcNow;
        return this;
    }

    private RefreshToken()
    {
        Value = string.Empty;
        ExpiresAt = DateTime.UtcNow;
    }


    public static class Factory
    {
        public static RefreshToken Create(string value, long userId, DateTime expiresAt)
        {
            return new RefreshToken()
                .SetValue(value)
                .SetUserId(userId)
                .SetExpiresAt(expiresAt);
        }
    }
}
