using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Cards.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Themes.Commands.UpdateTheme;
internal sealed class UpdateThemeHandler : ICommandHandler<UpdateThemeCommand, long>
{
    private readonly IRepository<Card> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public UpdateThemeHandler(IRepository<Card> repository,
              IUnitOfWork unitOfWork,
              IUserService userService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<long> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId();

        var card = await _repository.GetAsync(new FindCardWithThemeSpecification(cardId), cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.User.NotFound);

        card.Theme.SetName(request.Name)
                  .SetPrimaryColor(request.PrimaryColor)
                  .SetSecondaryColor(request.SecondaryColor)
                  .SetHasSharpEdges(request.HasSharpEdges)
                  .SetTextDark(request.TextDark)
                  .SetTextLight(request.TextLight)
                  .SetBackground(request.Background)
                  .SetTextOnButtons(request.TextOnButtons)
                  .SetRoundedProfilePicture(request.RoundedProfilePicture)
                  .SetLinkBackgroundColor(request.LinkBackgroundColor)
                  .SetLinkTextColor(request.LinkTextColor);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return cardId;
    }
}