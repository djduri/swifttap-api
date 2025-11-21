using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Commands.CreateCardVisitGeoStatistic;
using SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Commands.CreateCardVisitStatistic;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCard;
internal sealed class GetCardHandler : IQueryHandler<GetCardQuery, string>
{
    private readonly DatabaseContext _dbContext;
    private readonly FrontendUrlSettings _frontendUrlSettings;
    private readonly ISender _sender;
    private readonly ILogger<GetCardHandler> _logger;

    public GetCardHandler(DatabaseContext dbContext,
                          IOptions<FrontendUrlSettings> frontendUrlSettings,
                          ISender sender,
                          ILogger<GetCardHandler> logger)
    {
        _dbContext = dbContext;
        _frontendUrlSettings = frontendUrlSettings.Value;
        _sender = sender;
        _logger = logger;
    }

    public async Task<string> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        var card = await _dbContext.Cards.AsNoTracking()
                                         .Where(x => x.Guid == request.Guid)
                                         .SingleOrDefaultAsync(cancellationToken);

        if (card is null)
        {
            _logger.LogWarning("Card with GUID {Guid} not found.", request.Guid);
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
        }

        var redirectUrl = (_frontendUrlSettings.Url + _frontendUrlSettings.GetUserCard).Replace("{uniqueName}", card.UniqueName);

        await _sender.Send(new CreateCardVisitStatisticCommand(card.Id));
        await _sender.Send(new CreateCardVisitGeoStatisticCommand(card.Id));

        return redirectUrl;
    }
}
