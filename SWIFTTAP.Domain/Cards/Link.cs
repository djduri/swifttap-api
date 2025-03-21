using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;

namespace SWIFTTAP.Domain.Cards;
public sealed class Link : Entity
{
    public string Type { get; private set; }
    public string Name { get; private set; }
    public string Url { get; private set; }
    public long CardId { get; private set; }
    public Card Card { get; private set; }
    public IList<LinkVisitStatistic> LinkVisitStatistics { get; private set; }


    public Link SetType(string type)
    {
        if (type.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.Link.InvalidType);

        Type = type;
        return this;
    }

    public Link SetName(string name)
    {
        if (name.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.Link.InvalidName);

        Name = name;
        return this;
    }

    public Link SetUrl(string url)
    {
        if (url.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.Link.InvalidUrl);

        Url = url;
        return this;
    }

    public Link SetCardId(long cardId)
    { 
        if (cardId == default)
            throw DomainException.FromErrorCode(ErrorCodes.Link.InvalidCardId);

        CardId = cardId;
        return this;
    }

    private Link() 
    {
        LinkVisitStatistics = new List<LinkVisitStatistic>();
    }

    public static class Factory
    {
        public static Link Create(string name, string type, string url, long cardId)
        { 
            return new Link()
                .SetName(name)
                .SetType(type)
                .SetUrl(url)
                .SetCardId(cardId);
        }
    }
}
