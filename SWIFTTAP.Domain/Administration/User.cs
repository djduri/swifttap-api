using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Domain.Administration;

public sealed class User : IdentityEntity
{
    public string Name { get; private set ; }
    public UserKeys? UserKeys { get; private set; }
    public IList<RefreshToken> RefreshTokens { get; private set; }
    public Card Card { get; private set; }

    public User SetName(string name)
    {
        if (name.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.User.InvalidName);

        Name = name;
        return this;
    }

    public User SetEmail(string email)
    {
        if (email.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.User.InvalidEmail);

        Email = email;
        return this;
    }

    public User SetUserName(string email)
    {
        if (email.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.User.InvalidEmail);

        UserName = email;
        return this;
    }

    public User SetEmailConfirmed(bool emailConfirmed)
    {
        EmailConfirmed = emailConfirmed;
        return this;
    }

    public User SetTwoFactorEnabled(bool twoFactorEnabled)
    { 
        TwoFactorEnabled = twoFactorEnabled;
        return this;
    }

    public User RevokeAllRefreshTokens()
    {
        foreach (var refreshToken in RefreshTokens)
            refreshToken.Revoke();
        return this;
    }

    private User SetCard(Card card)
    {
        Card = card;
        return this;
    }

    private User()
    {
        UserKeys = new UserKeys();
        RefreshTokens = new List<RefreshToken>();
    }

    public static class Factory
    {
        public static User Create(string name,                                         
                                  string email,
                                  Card card)
        {
            return new User()
                .SetName(name)
                .SetCard(card)
                .SetEmail(email)
                .SetUserName(email)
                .SetEmailConfirmed(false);
        }

        public static User CreateWithConfirmedEmail(string name,
                                                    string email,
                                                    Card card)
        {
            return new User()
                .SetName(name)
                .SetCard(card)
                .SetEmail(email)
                .SetUserName(email)
                .SetEmailConfirmed(true);
        }
    }
}
