using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLinkAsAdmin;
internal sealed class GetNfcLinkAsAdminHandler : IQueryHandler<GetNfcLinkAsAdminQuery, string>
{
    private readonly DatabaseContext _dbContext;
    private readonly ApiUrlSettings _apiUrlSettings;
    private readonly ILogger<GetNfcLinkAsAdminHandler> _logger;

    public GetNfcLinkAsAdminHandler(DatabaseContext dbContext, IOptions<ApiUrlSettings> apiUrlSettings, ILogger<GetNfcLinkAsAdminHandler> logger)
    {
        _dbContext = dbContext;
        _apiUrlSettings = apiUrlSettings.Value;
        _logger = logger;
    }

    public async Task<string> Handle(GetNfcLinkAsAdminQuery request, CancellationToken cancellationToken)
    {
        var guid = await _dbContext.Cards.AsNoTracking()
                                         .Where(x => x.Id == request.CardId)
                                         .Select(x => x.Guid)
                                         .SingleOrDefaultAsync(cancellationToken);

        if (guid == default)
        {
            _logger.LogWarning("Card with ID {CardId} not found.", request.CardId);
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
        }

        var nfcLink = (_apiUrlSettings.Url + _apiUrlSettings.NfcLink).Replace("{guid}", guid.ToString());

        return nfcLink;
    }
}
