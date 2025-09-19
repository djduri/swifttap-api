using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Commands.UpdateCard;
internal sealed class UpdateCardHandler : ICommandHandler<UpdateCardCommand, long>
{
    private readonly IRepository<Card> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly ILogger<UpdateCardHandler> _logger;

    public UpdateCardHandler(IRepository<Card> repository,
                             IUnitOfWork unitOfWork,
                             IUserService userService,
                             ILogger<UpdateCardHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _logger = logger;
    }

    public async Task<long> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        if (!_userService.HasAuthUserPermissionToCard(request.CardId))
        {
            _logger.LogWarning("Unauthorized attempt to update a card with ID {TargetCardId}.", request.CardId);
            throw AuthorizationException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        var card = await _repository.GetAsync(request.CardId, cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);

        card.SetEmail(request.Email)
            .SetPhoneNumber(request.PhoneNumber)
            .SetDescription(request.Description)
            .SetIsEmailShareable(request.IsEmailShareable)
            .SetIsPhoneNumberShareable(request.IsPhoneNumberShareable);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return request.CardId;
    }
}