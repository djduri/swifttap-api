using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Links.Commands.DeleteLink;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.DeleteLink;
internal sealed class DeleteLinkHandler : ICommandHandler<DeleteLinkCommand, long>
{
    private readonly IRepository<Link> _repository;
    private readonly ILogger<DeleteLinkHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public DeleteLinkHandler(IRepository<Link> repository,
                             ILogger<DeleteLinkHandler> logger,
                             IUnitOfWork unitOfWork,
                             IUserService userService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _logger = logger;
    }

    public async Task<long> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId();

        var link = await _repository.GetAsync(request.Id, cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Link.NotFound);

        if (!_userService.IsAuthenticatedUserAdmin() && cardId != link.CardId)
        {
            _logger.LogWarning("A user tried to delete a link with ID {LinkId} that does not belong to them. Access denied.", link.Id);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        _repository.Delete(link);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
