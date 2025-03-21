using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLink;
internal sealed class GetNfcLinkHandler : IQueryHandler<GetNfcLinkQuery, string>
{
    private readonly DatabaseContext _dbContext;
    private readonly ApiUrlSettings _apiUrlSettings;
    private readonly IUserService _userService;
    private readonly ILogger<GetNfcLinkHandler> _logger;

    public GetNfcLinkHandler(DatabaseContext dbContext, IOptions<ApiUrlSettings> apiUrlSettings, IUserService userService, ILogger<GetNfcLinkHandler> logger)
    {
        _dbContext = dbContext;
        _apiUrlSettings = apiUrlSettings.Value;
        _userService = userService;
        _logger = logger;
    }

    public async Task<string> Handle(GetNfcLinkQuery request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId();

        var guid = await _dbContext.Cards.AsNoTracking()
                                         .Where(x => x.Id == cardId)
                                         .Select(x => x.Guid)
                                         .SingleOrDefaultAsync(cancellationToken);

        if (guid == default)
        {
            // Warning log
            _logger.LogWarning("Card with ID {CardId} not found.", cardId);
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
        }

        var nfcLink = (_apiUrlSettings.Url + _apiUrlSettings.NfcLink).Replace("{guid}", guid.ToString());

        return nfcLink;
    }
}
