using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Queries.GetSampledCardVisitStatistics;
internal sealed class GetSampledCardVisitStatisticsHandler : IQueryHandler<GetSampledCardVisitStatisticsQuery, IEnumerable<VisitStatisticSampledDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IUserService _userService;
    private readonly ILogger<GetSampledCardVisitStatisticsHandler> _logger;
    private readonly IStatisticsService _statisticsService;

    public GetSampledCardVisitStatisticsHandler(DatabaseContext dbContext,
                                                IUserService userService,
                                                ILogger<GetSampledCardVisitStatisticsHandler> logger,
                                                IStatisticsService statisticsService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _logger = logger;
        _statisticsService = statisticsService;
    }

    public async Task<IEnumerable<VisitStatisticSampledDTO>> Handle(GetSampledCardVisitStatisticsQuery request, CancellationToken cancellationToken)
    {
        if (!_userService.HasAuthUserPermissionToCard(request.CardId))
        {
            _logger.LogWarning("Access denied for non-admin user trying to access card statistics. CardId: {CardId}", request.CardId);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        if (!await _dbContext.Cards.AnyAsync(x => x.Id == request.CardId, cancellationToken))
        {
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
        }

        var visitStatistics = await _dbContext.CardVisitStatistics.AsNoTracking()
                                                                       .Where(x => x.CardId == request.CardId
                                                                                   && x.Date >= request.StartDate
                                                                                   && x.Date <= request.EndDate)
                                                                       .Select(x => new VisitStatistic
                                                                       {
                                                                           Date = x.Date,
                                                                           Counter = x.Counter,
                                                                       })
                                                                       .ToListAsync(cancellationToken);
    

        return _statisticsService.ApplySampling(visitStatistics,
                                                request.StartDate,
                                                request.EndDate,
                                                request.NumberOfSamples);
    }   
}
