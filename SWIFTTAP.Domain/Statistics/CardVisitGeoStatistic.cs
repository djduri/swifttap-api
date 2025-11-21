using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Statistics;

public class CardVisitGeoStatistic: Entity
{
    public long CardId { get; private set; }
    public Card Card { get; private set; }

    public DateOnly Date { get; private set; }

    public string City { get; private set; }
    public string CountryCode {  get; private set; }


    public CardVisitGeoStatistic SetCardId(long cardId)
    {
        if (cardId == default)
            throw DomainException.FromErrorCode(ErrorCodes.CardVisitGeoStatistic.InvalidCardId);

        CardId = cardId;
        return this;
    }

    public CardVisitGeoStatistic SetCity(string city)
    {
        if (string.IsNullOrEmpty(city))
            throw DomainException.FromErrorCode(ErrorCodes.CardVisitGeoStatistic.InvalidCity);

        City = city;
        return this;
    }

    public CardVisitGeoStatistic SetCountryCode(string countryCode)
    {
        if (string.IsNullOrEmpty(countryCode))
            throw DomainException.FromErrorCode(ErrorCodes.CardVisitGeoStatistic.InvalidCountryCode);

        CountryCode = countryCode;
        return this;
    }


    private CardVisitGeoStatistic()
    {
        Date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static class Factory
    {
        public static CardVisitGeoStatistic Create(long cardId, string countryCode, string city)
        {
            return new CardVisitGeoStatistic()
                .SetCardId(cardId)
                .SetCountryCode(countryCode)
                .SetCity(city);
        }
    }
}
