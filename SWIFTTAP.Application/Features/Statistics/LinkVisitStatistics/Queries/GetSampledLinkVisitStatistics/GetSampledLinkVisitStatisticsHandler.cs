using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Queries.GetSampledLinkVisitStatistics;
internal sealed class GetSampledLinkVisitStatisticsHandler : IQueryHandler<GetSampledLinkVisitStatisticsQuery, IEnumerable<VisitStatisticSampledDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IUserService _userService;
    private readonly ILogger<GetSampledLinkVisitStatisticsHandler> _logger;
    private readonly IStatisticsService _statisticsService;

    public GetSampledLinkVisitStatisticsHandler(DatabaseContext dbContext,
                                                IUserService userService,
                                                ILogger<GetSampledLinkVisitStatisticsHandler> logger,
                                                IStatisticsService statisticsService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _logger = logger;
        _statisticsService = statisticsService;
    }

    public async Task<IEnumerable<VisitStatisticSampledDTO>> Handle(GetSampledLinkVisitStatisticsQuery request, CancellationToken cancellationToken)
    {
        if (_userService.IsAuthenticatedUserNotAdmin())
        {
            var cardId = _userService.GetAuthenticatedUserCardId();
            if (!await _dbContext.Links.AnyAsync(x => x.Id == request.LinkId && x.CardId == cardId, cancellationToken))
            {
                _logger.LogWarning("Access denied for user with card ID {CardId} on link {LinkId}.", cardId, request.LinkId);
                throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
            }
        }

        else if (!await _dbContext.Links.AnyAsync(x => x.Id == request.LinkId, cancellationToken))
        { 
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Link.NotFound);
        }

        var visitStatistics = await _dbContext.LinkVisitStatistics.AsNoTracking()
                                                                       .Where(x => x.LinkId == request.LinkId
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
