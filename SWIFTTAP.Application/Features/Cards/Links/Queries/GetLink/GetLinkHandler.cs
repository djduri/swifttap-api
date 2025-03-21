using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Links.DTOs;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.Links.Queries.GetLink;
internal sealed class GetLinkHandler : IQueryHandler<GetLinkQuery, LinkDetailsDTO>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public GetLinkHandler(DatabaseContext dbContext, IMapper mapper, IUserService userService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<LinkDetailsDTO> Handle(GetLinkQuery request, CancellationToken cancellationToken)
    {
        var link = await _dbContext.Links.AsNoTracking().SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Link.NotFound);

        var cardId = _userService.GetAuthenticatedUserCardId();       

        if (!_userService.IsAuthenticatedUserAdmin() && cardId != link.CardId)
        {
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        return _mapper.Map<LinkDetailsDTO>(link);
    }
}
