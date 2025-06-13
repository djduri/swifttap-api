using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Queries.GetSampledLinkVisitStatistics;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Queries.GetSampledVcfDownloadStatistics;
internal sealed class GetSampledVcfDownloadStatisticsHandler : IQueryHandler<GetSampledVcfDownloadStatisticsQuery, IEnumerable<VisitStatisticSampledDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IUserService _userService;
    private readonly ILogger<GetSampledLinkVisitStatisticsHandler> _logger;
    private readonly IStatisticsService _statisticsService;

    public GetSampledVcfDownloadStatisticsHandler(DatabaseContext dbContext,
                                                IUserService userService,
                                                ILogger<GetSampledLinkVisitStatisticsHandler> logger,
                                                IStatisticsService statisticsService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _logger = logger;
        _statisticsService = statisticsService;
    }

    public async Task<IEnumerable<VisitStatisticSampledDTO>> Handle(GetSampledVcfDownloadStatisticsQuery request, CancellationToken cancellationToken)
    {
        var card = await _dbContext.Cards.SingleOrDefaultAsync(x => x.UniqueName == request.UniqueName, cancellationToken);

        if (_userService.IsAuthenticatedUserNotAdmin())
        {
            var cardId = _userService.GetAuthenticatedUserCardId();
            if (card is null || card.Id != cardId)
            {
                _logger.LogWarning("Access denied for user with card ID {CardId} on UniqueName {UniqueName}.", cardId, request.UniqueName);
                throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
            }
        }

        else if (card is null)
        {
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
        }

        var vcfDownloadStatistic = await _dbContext.VcfDownloadStatistics.AsNoTracking()
                                                                         .Where(x => x.CardId == card.Id
                                                                                     && x.Date >= request.StartDate
                                                                                     && x.Date <= request.EndDate)
                                                                         .Select(x => new VisitStatistic
                                                                         {
                                                                             Date = x.Date,
                                                                             Counter = x.Counter,
                                                                         })
                                                                         .ToListAsync(cancellationToken);


        return _statisticsService.ApplySampling(vcfDownloadStatistic,
                                                request.StartDate,
                                                request.EndDate,
                                                request.NumberOfSamples);
    }
}