using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Statistics;
public sealed class CardVisitStatistic : Entity
{
    public long CardId { get; private set; }
    public Card Card { get; private set; }

    public DateOnly Date { get; private set; }
    public int Counter { get; private set; }

    public CardVisitStatistic SetCardId(long cardId)
    {
        if (cardId == default)
            throw DomainException.FromErrorCode(ErrorCodes.CardVisitStatistic.InvalidCardId);

        CardId = cardId;
        return this;
    }

    public CardVisitStatistic IncrementCounter()
    {
        Counter++;
        return this;
    }

    private CardVisitStatistic() 
    {
        Counter = 0;
        Date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static class Factory
    {
        public static CardVisitStatistic Create(long cardId)
        {
            return new CardVisitStatistic()
                .SetCardId(cardId);
        }
    }
}
//TODO DO STATYSTYK Z CHATA
/*
private IEnumerable<CardVisitStatistic> ApplySampling(IEnumerable<CardVisitStatistic> statistics, DateTime startDate, DateTime endDate, int numberOfSamples)
{
    var sampledStats = new List<CardVisitStatistic>();

    // Zakładamy, że dane są posortowane rosnąco według daty
    var totalDays = (endDate - startDate).Days;

    // Obliczamy, jak długo będzie każdy okres próbki (w dniach)
    int sampleIntervalDays = totalDays / numberOfSamples;

    // Jeśli interwał próbkujący jest mniejszy niż 1 dzień, to próbkujemy codziennie
    if (sampleIntervalDays < 1)
    {
        sampleIntervalDays = 1; // próbkuj codziennie
    }

    // Teraz próbkujemy w odpowiednich odstępach
    for (var i = 0; i < numberOfSamples; i++)
    {
        // Oblicz datę początkową dla danego okresu próbki
        var sampleStartDate = startDate.AddDays(i * sampleIntervalDays);
        // Oblicz datę końcową dla danego okresu próbki
        var sampleEndDate = sampleStartDate.AddDays(sampleIntervalDays - 1);

        // Wyszukaj dane w tym przedziale czasowym
        var statisticsForSample = statistics
            .Where(x => x.Date >= sampleStartDate && x.Date <= sampleEndDate)
            .ToList();

        // Jeśli są jakieś dane dla tej daty, oblicz średnią
        if (statisticsForSample.Any())
        {
            var averageStatistic = CalculateAverageStatistic(statisticsForSample);
            sampledStats.Add(averageStatistic);
        }
    }

    return sampledStats;
}

private CardVisitStatistic CalculateAverageStatistic(IEnumerable<CardVisitStatistic> statisticsForPeriod)
{
    // Obliczanie średniej z danych w tym okresie
    var averageCounter = statisticsForPeriod.Average(x => x.Counter);
    
    // Tworzymy nową statystykę z uśrednioną liczbą wejść
    var averageStatistic = statisticsForPeriod.First(); // Możemy wybrać dowolną datę w tym okresie (np. pierwszy dzień próbki)
    return new CardVisitStatistic
    {
        CardId = averageStatistic.CardId,
        Date = averageStatistic.Date, // Możemy ustawić np. pierwszy dzień próbki
        Counter = (int)averageCounter
    };
}


 */