using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Statistics;
public sealed class VcfDownloadStatistic : Entity
{
    public long CardId { get; private set; }
    public Card Card { get; private set; }

    public DateOnly Date { get; private set; }
    public int Counter { get; private set; }

    public VcfDownloadStatistic SetCardId(long cardId)
    {
        if (cardId == default)
            throw DomainException.FromErrorCode(ErrorCodes.VcfDownloadStatistic.InvalidCardId);

        CardId = cardId;
        return this;
    }

    public VcfDownloadStatistic IncrementCounter()
    {
        Counter++;
        return this;
    }

    private VcfDownloadStatistic()
    {
        Counter = 0;
        Date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static class Factory
    {
        public static VcfDownloadStatistic Create(long cardId)
        {
            return new VcfDownloadStatistic()
                .SetCardId(cardId);
        }
    }
}
