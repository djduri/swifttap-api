using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.DTOs;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;
using System.Globalization;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Queries.GetSampledCardVisitGeoStatisticsByCity;

internal sealed class GetSampledCardVisitGeoStatisticsByCityHandler : IQueryHandler<GetSampledCardVisitGeoStatisticsByCityQuery, IEnumerable<CityVisitStatisticsDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IUserService _userService;
    private readonly ILogger<GetSampledCardVisitGeoStatisticsByCityHandler> _logger;

    public GetSampledCardVisitGeoStatisticsByCityHandler(DatabaseContext dbContext,
                                                         IUserService userService,
                                                         ILogger<GetSampledCardVisitGeoStatisticsByCityHandler> logger)
    {
        _dbContext = dbContext;
        _userService = userService;
        _logger = logger;
    }

    public async Task<IEnumerable<CityVisitStatisticsDTO>> Handle(GetSampledCardVisitGeoStatisticsByCityQuery request, CancellationToken cancellationToken)
    {
        // Sprawdzenie uprawnień w zależności od tego czy podano CardId
        if (request.CardId.HasValue)
        {
            // Non-admin może zobaczyć tylko swoją kartę
            if (!_userService.HasAuthUserPermissionToCard(request.CardId.Value))
            {
                _logger.LogWarning("Access denied for non-admin user trying to access card geo statistics. CardId: {CardId}", request.CardId);
                throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
            }

            // Sprawdzenie, czy podana karta istnieje
            if (!await _dbContext.Cards.AnyAsync(x => x.Id == request.CardId, cancellationToken))
            {
                _logger.LogWarning("Not found card CardId: {CardId}", request.CardId);
                throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
            }
        }
        else
        {
            // Brak CardId – tylko admin może zobaczyć dla wszystkich kart
            if (!_userService.IsAuthenticatedUserAdmin())
            {
                _logger.LogWarning("Access denied for non-admin user trying to access all cards geo statistics.");
                throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
            }
        }

        // Pobranie danych z bazy, filtrowanie po CardId jeśli podano
        var entriesQuery = _dbContext.CardVisitGeoStatistics.AsQueryable();
        if (request.CardId.HasValue)
            entriesQuery = entriesQuery.Where(x => x.CardId == request.CardId.Value);

        // Filtrowanie po zakresie dat
        entriesQuery = entriesQuery.Where(x => x.Date >= request.StartDate && x.Date <= request.EndDate);
        var entries = await entriesQuery.ToListAsync(cancellationToken);

        if (!entries.Any())
            return Enumerable.Empty<CityVisitStatisticsDTO>();

        // Wyciągamy listę unikalnych miast i krajów
        var cities = entries.Select(x => new { x.City, x.CountryCode }).Distinct().ToList();
        var result = new List<CityVisitStatisticsDTO>();

        foreach (var city in cities)
        {
            // Pobranie wszystkich wpisów dla konkretnego miasta
            var cityEntries = entries.Where(e => e.City == city.City && e.CountryCode == city.CountryCode).ToList();

            var cityDto = new CityVisitStatisticsDTO
            {
                City = city.City,
                Country = city.CountryCode,
                Visits = new List<PeriodVisitsDTO>()
            };

            // Generowanie wszystkich okresów (dni, tygodni, miesięcy) jeśli IncludeEmptyPeriods = true
            var periods = new List<string>();
            if (request.IncludeEmptyPeriods)
            {
                switch (request.SampleType)
                {
                    case SampleType.Day:
                        var currentDay = request.StartDate;
                        while (currentDay <= request.EndDate)
                        {
                            periods.Add(currentDay.ToString("yyyy-MM-dd"));
                            currentDay = currentDay.AddDays(1);
                        }
                        break;

                    case SampleType.Week:
                        var currentWeek = request.StartDate;
                        while (currentWeek <= request.EndDate)
                        {
                            var week = ISOWeek.GetWeekOfYear(currentWeek.ToDateTime(TimeOnly.MinValue));
                            periods.Add($"{currentWeek.Year}-W{week}");
                            currentWeek = currentWeek.AddDays(7);
                        }
                        break;

                    case SampleType.Month:
                        var currentMonth = request.StartDate;
                        while (currentMonth <= request.EndDate)
                        {
                            periods.Add($"{currentMonth.Year}-{currentMonth.Month:D2}");
                            currentMonth = currentMonth.AddMonths(1);
                        }
                        break;
                }
            }

            // Grupowanie istniejących wpisów w okresach
            var grouped = request.SampleType switch
            {
                SampleType.Day => cityEntries
                    .GroupBy(e => e.Date.ToString("yyyy-MM-dd"))
                    .Select(g => new PeriodVisitsDTO { Period = g.Key, Count = g.Count() })
                    .ToList(),

                SampleType.Week => cityEntries
                    .GroupBy(e => {
                        var dt = e.Date.ToDateTime(TimeOnly.MinValue);
                        return $"{dt.Year}-W{ISOWeek.GetWeekOfYear(dt)}";
                    })
                    .Select(g => new PeriodVisitsDTO { Period = g.Key, Count = g.Count() })
                    .ToList(),

                SampleType.Month => cityEntries
                    .GroupBy(e => $"{e.Date.Year}-{e.Date.Month:D2}")
                    .Select(g => new PeriodVisitsDTO { Period = g.Key, Count = g.Count() })
                    .ToList(),

                _ => new List<PeriodVisitsDTO>()
            };

            // Dodanie brakujących okresów z liczbą 0 jeśli IncludeEmptyPeriods = true
            if (request.IncludeEmptyPeriods)
            {
                foreach (var p in periods)
                {
                    if (!grouped.Any(g => g.Period == p))
                        grouped.Add(new PeriodVisitsDTO { Period = p, Count = 0 });
                }
            }

            // Dodanie danych do DTO i posortowanie po okresie
            cityDto.Visits.AddRange(grouped.OrderBy(g => g.Period));
            result.Add(cityDto);
        }

        return result;
    }
}
