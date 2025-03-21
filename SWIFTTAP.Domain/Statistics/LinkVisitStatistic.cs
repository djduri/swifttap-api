using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Statistics;
public sealed class LinkVisitStatistic : Entity
{
    public long LinkId { get; private set; }
    public Link Link { get; private set; }

    public DateOnly Date { get; private set; }
    public int Counter { get; private set; }

    public LinkVisitStatistic SetLinkId(long linkId)
    {
        if (linkId == default)
            throw DomainException.FromErrorCode(ErrorCodes.LinkVisitStatistic.InvalidLinkId);

        LinkId = linkId;
        return this;
    }

    public LinkVisitStatistic IncrementCounter()
    {
        Counter++;
        return this;
    }

    private LinkVisitStatistic()
    {
        Counter = 0;
        Date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static class Factory
    {
        public static LinkVisitStatistic Create(long linkId)
        {
            return new LinkVisitStatistic()
                .SetLinkId(linkId);
        }
    }
}

