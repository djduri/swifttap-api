using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Domain.Cards;
public sealed class Card : Entity
{
    public long UserId { get; private set; }
    public User User { get; private set; }
    public Guid Guid { get; private set; }
    public string UniqueName { get; private set; }
    public bool HasLogo { get; private set; }
    public Logo Logo { get; private set; }
    public Theme Theme { get; private set; }
    public IList<Link> Links { get; private set; }
    public IList<CardVisitStatistic> CardVisitStatistics { get; private set; }


    public Card SetUniqueName(string uniqueName)
    {
        if (uniqueName.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.Card.InvalidUniqueName);

        UniqueName = uniqueName;
        return this;
    }

    public Card SetHasLogo(bool hasLogo)
    {
        HasLogo = hasLogo;
        return this;
    }

    internal Card()
    {
        Guid = Guid.NewGuid();
        Theme = new Theme();
        Logo = new Logo();        
        Links = new List<Link>();
        CardVisitStatistics = new List<CardVisitStatistic>();
    }

    public static class Factory
    {
        public static Card Create(string uniqueName)
        {
            return new Card()
                .SetUniqueName(uniqueName);
        }
    }
}
