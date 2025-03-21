using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Cards.DTOs;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCardByUniqueName;
internal sealed class GetCardByUniqueNameHandler : IQueryHandler<GetCardByUniqueNameQuery, CardDetailsDTO>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCardByUniqueNameHandler> _logger;

    public GetCardByUniqueNameHandler(DatabaseContext dbContext, IMapper mapper, ILogger<GetCardByUniqueNameHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CardDetailsDTO> Handle(GetCardByUniqueNameQuery request, CancellationToken cancellationToken)
    {
        var card = await _dbContext.Cards.AsNoTracking()
                                         .Include(x => x.User)
                                         .Include(x => x.Theme)
                                         .Include(x => x.Links)
                                         .SingleOrDefaultAsync(x => x.UniqueName == request.UniqueName, cancellationToken);
        if (card is null)
        {
            _logger.LogWarning("Card with UniqueName {UniqueName} not found.", request.UniqueName);
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
        }

        return _mapper.Map<CardDetailsDTO>(card);
    }
}
