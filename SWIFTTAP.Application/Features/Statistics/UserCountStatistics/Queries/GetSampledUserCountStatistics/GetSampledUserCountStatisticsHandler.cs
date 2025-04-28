using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Statistics.UserCountStatistics.DTOs;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Statistics.UserCountStatistics.Queries.GetSampledUserCountStatistics;
internal sealed class GetSampledUserCountStatisticsHandler : IQueryHandler<GetSampledUserCountStatisticsQuery, IEnumerable<UserCountStatisticSampleDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IUserService _userService;
    private readonly ILogger<GetSampledUserCountStatisticsHandler> _logger;

    public GetSampledUserCountStatisticsHandler(DatabaseContext dbContext,
                                                IUserService userService,
                                                ILogger<GetSampledUserCountStatisticsHandler> logger)
    {
        _dbContext = dbContext;
        _userService = userService;
        _logger = logger;
    }

    public async Task<IEnumerable<UserCountStatisticSampleDTO>> Handle(GetSampledUserCountStatisticsQuery request, CancellationToken cancellationToken)
    {
        // Pobieramy dane o liczbie użytkowników w zadanym przedziale czasu
        var userStatistics = await _dbContext.UserCountStatistics
                                             .AsNoTracking()
                                             .Where(x => x.Date >= request.StartDate && x.Date <= request.EndDate)
                                             .Select(x => new
                                             {
                                                 Date = x.Date,
                                                 Counter = x.Counter
                                             })
                                             .ToListAsync(cancellationToken);

        var result = new List<UserCountStatisticSampleDTO>();

        // Tworzymy listę wszystkich dat w wybranym przedziale czasowym
        var allDatesInPeriod = Enumerable.Range(0, (request.EndDate.ToDateTime(TimeOnly.MinValue) - request.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1)
                                         .Select(offset => request.StartDate.AddDays(offset))
                                         .ToList();

        // Przekształcamy pobrane dane na słownik: data -> liczba użytkowników
        var statisticsDictionary = userStatistics.ToDictionary(x => x.Date, x => x.Counter);

        // Tworzymy pełną listę statystyk, gdzie brakujące dni uzupełniamy zerami
        var statisticsWithZeros = allDatesInPeriod.Select(date =>
        {
            if (statisticsDictionary.TryGetValue(date, out var counter))
            {
                return new { Date = date, Counter = counter };
            }
            else
            {
                return new { Date = date, Counter = 0 };
            }
        }).ToList();

        var totalDays = allDatesInPeriod.Count;

        // Obliczamy co ile dni ma być próbka (interwał)
        int sampleIntervalDays = totalDays / request.NumberOfSamples;

        // Jeżeli interwał próbki wynosi mniej niż 1 dzień, ustawiamy 1 dzień
        if (sampleIntervalDays < 1)
        {
            sampleIntervalDays = 1;
        }

        // Przechodzimy przez wszystkie próbki
        for (var i = 0; i < request.NumberOfSamples; i++)
        {
            // Ustalamy początkową datę próbki
            var sampleStartDate = request.StartDate.AddDays(i * sampleIntervalDays);

            // Ustalamy końcową datę próbki (ostatnia próbka może obejmować więcej dni)
            var sampleEndDate = (i == request.NumberOfSamples - 1)
                ? request.EndDate
                : sampleStartDate.AddDays(sampleIntervalDays - 1);

            // Pobieramy dane statystyczne dla bieżącej próbki
            var statisticsForSample = statisticsWithZeros
                .Where(x => x.Date >= sampleStartDate && x.Date <= sampleEndDate)
                .ToList();

            // Jeśli są dane dla próbki, obliczamy średnią i sumę
            if (statisticsForSample.Any())
            {
                var averageUsers = statisticsForSample.Average(x => x.Counter);
                var totalUsers = statisticsForSample.Sum(x => x.Counter);
                var medianDate = statisticsForSample.ElementAt(statisticsForSample.Count / 2).Date;

                // Tworzymy DTO reprezentujące próbkę
                var sample = new UserCountStatisticSampleDTO
                {
                    Date = medianDate, // Środek próbki (mediana)
                    AverageUsers = (int)Math.Round(averageUsers, MidpointRounding.AwayFromZero),
                    TotalUsers = totalUsers
                };

                // Dodajemy próbkę do wyniku
                result.Add(sample);
            }
        }

        // Zwracamy wszystkie próbki
        return result;
    }
}



