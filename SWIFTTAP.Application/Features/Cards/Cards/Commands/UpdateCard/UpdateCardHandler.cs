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

    public UpdateCardHandler(IRepository<Card> repository,
                             IUnitOfWork unitOfWork,
                             IUserService userService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<long> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId();

        var card = await _repository.GetAsync(cardId, cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);

        card.SetEmail(request.Email)
            .SetPhoneNumber(request.PhoneNumber)
            .SetIsEmailShareable(request.IsEmailShareable)
            .SetIsPhoneNumberShareable(request.IsPhoneNumberShareable);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cardId;
    }
}