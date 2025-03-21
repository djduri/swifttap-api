using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Queries.GetSampledAllCardVisitStatistics;
internal sealed class GetSampledAllCardVisitStatisticsHandler : IQueryHandler<GetSampledAllCardVisitStatisticsQuery, IEnumerable<VisitStatisticSampledDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IStatisticsService _statisticsService;

    public GetSampledAllCardVisitStatisticsHandler(DatabaseContext dbContext,
                                                IStatisticsService statisticsService)
    {
        _dbContext = dbContext;
        _statisticsService = statisticsService;
    }

    public async Task<IEnumerable<VisitStatisticSampledDTO>> Handle(GetSampledAllCardVisitStatisticsQuery request, CancellationToken cancellationToken)
    {
        var visitStatistics = await _dbContext.CardVisitStatistics.AsNoTracking()
                                                                  .Where(x => x.Date >= request.StartDate && x.Date <= request.EndDate)
                                                                  .GroupBy(x => x.Date) // Grupa po dacie
                                                                  .Select(g => new VisitStatistic
                                                                  {
                                                                      Date = g.Key, // Data
                                                                      Counter = g.Sum(x => x.Counter) // Suma liczby odwiedzin dla danego dnia
                                                                  })
                                                                  .ToListAsync(cancellationToken);

        return _statisticsService.ApplySampling(visitStatistics,
                                                request.StartDate,
                                                request.EndDate,
                                                request.NumberOfSamples);
    }
}
