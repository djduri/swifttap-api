using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Links.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.UpdateLink;
internal sealed class UpdateLinkHandler : ICommandHandler<UpdateLinkCommand, long>
{
    private readonly IRepository<Link> _repository;
    private readonly ILogger<UpdateLinkHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public UpdateLinkHandler(IRepository<Link> repository,
                             ILogger<UpdateLinkHandler> logger,
                             IUnitOfWork unitOfWork,
                             IUserService userService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _logger = logger;
    }

    public async Task<long> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId();

        var link = await _repository.GetAsync(request.Id, cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Link.NotFound);

        // Sprawdzamy, czy użytkownik jest administratorem lub jest właścicielem linku
        if (!_userService.IsAuthenticatedUserAdmin() && cardId != link.CardId)
        {
            // Dodanie logowania, gdy użytkownik nie jest administratorem i próbuje zmienić link, który nie należy do niego
            _logger.LogWarning("A user tried to update a link with ID {LinkId} that does not belong to them. Access denied.", link.Id);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }       

        // Zmiana danych linku
        link.SetName(request.Name)
            .SetType(request.Type)
            .SetUrl(request.Url)
            .SetOrder(request.Order);

        // Zapisanie zmian do bazy danych
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return link.Id;
    }
}
