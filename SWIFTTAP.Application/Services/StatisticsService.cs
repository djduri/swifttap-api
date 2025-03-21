using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Services.Interfaces;

namespace SWIFTTAP.Application.Services;
public class StatisticsService : IStatisticsService
{
    public StatisticsService()
    {
    }

    // Metoda próbkująca dla ogólnych statystyk odwiedzin
    public IList<VisitStatisticSampledDTO> ApplySampling(IEnumerable<VisitStatistic> visitStatistics,
                                                         DateOnly startDate,
                                                         DateOnly endDate,
                                                         int samplesCount)
    {
        var result = new List<VisitStatisticSampledDTO>();

        var allDatesInPeriod = Enumerable.Range(0, (endDate.ToDateTime(TimeOnly.MinValue).Subtract(startDate.ToDateTime(TimeOnly.MinValue))).Days + 1)
                                         .Select(offset => startDate.AddDays(offset))
                                         .ToList();

        // Tworzymy słownik, gdzie kluczem jest data, a wartością liczba odwiedzin (Counter)
        var statisticsDictionary = visitStatistics.ToDictionary(x => x.Date, x => x.Counter);

        // Tworzymy listę z danymi i zerami, w zależności od tego, czy dla danej daty istnieją dane
        var statisticsWithZeros = allDatesInPeriod.Select(date =>
        {
            if (statisticsDictionary.TryGetValue(date, out var counter))
            {
                return new VisitStatistic { Date = date, Counter = counter };
            }
            else
            {
                return new VisitStatistic { Date = date, Counter = 0 };
            }
        }).ToList();

        var totalDays = allDatesInPeriod.Count;

        // Obliczamy, jak długo będzie każdy okres próbki (w dniach)
        int sampleIntervalDays = totalDays / samplesCount;

        // Jeśli interwał próbkujący jest mniejszy niż 1 dzień, to próbkuj codziennie
        if (sampleIntervalDays < 1)
        {
            sampleIntervalDays = 1; // próbkuj codziennie
        }

        for (var i = 0; i < samplesCount; i++)
        {
            var sampleStartDate = startDate.AddDays(i * sampleIntervalDays);
            // Jeśli jesteśmy na ostatniej próbce, rozciągamy ją na wszystkie pozostałe dni
            var sampleEndDate = (i == samplesCount - 1)
                ? endDate
                : sampleStartDate.AddDays(sampleIntervalDays - 1);

            var statisticsForSample = statisticsWithZeros
                .Where(x => x.Date >= sampleStartDate && x.Date <= sampleEndDate)
                .ToList();

            if (statisticsForSample.Any())
            {
                var resultStatistic = CalculateStatistic(statisticsForSample);
                result.Add(resultStatistic);
            }
        }

        return result;
    }

    private VisitStatisticSampledDTO CalculateStatistic(IEnumerable<VisitStatistic> statisticsForSamplePeriod)
    {
        var averageVisits = statisticsForSamplePeriod.Average(x => x.Counter);
        var totalVisits = statisticsForSamplePeriod.Sum(x => x.Counter);

        var medianDate = statisticsForSamplePeriod.ElementAt(statisticsForSamplePeriod.Count() / 2).Date;

        return new VisitStatisticSampledDTO
        {
            Date = medianDate, // Wybieramy medianę daty
            AverageVisits = (int)Math.Round(averageVisits, MidpointRounding.AwayFromZero),
            TotalVisits = totalVisits
        };
    }
}
