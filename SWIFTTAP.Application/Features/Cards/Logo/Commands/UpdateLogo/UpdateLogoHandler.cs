using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Cards.Specifications;
using SWIFTTAP.Application.Helpers;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Logo.Commands.UpdateLogo;
internal sealed class UpdateLogoHandler : ICommandHandler<UpdateLogoCommand, long>
{
    private const int MaxIconWidth = 250;
    private const int MaxIconHeight = 250;

    private readonly IRepository<Card> _cardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IUserService _userService;
    private readonly ILogger<UpdateLogoHandler> _logger;

    public UpdateLogoHandler(IRepository<Card> cardRepository,
                             IUnitOfWork unitOfWork,
                             IImageService imageService,
                             IUserService userService,
                             ILogger<UpdateLogoHandler> logger)
    {
        _cardRepository = cardRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _userService = userService;
        _logger = logger;
    }


    public async Task<long> Handle(UpdateLogoCommand request, CancellationToken cancellationToken)
    {
        if (!_userService.HasAuthUserPermissionToCard(request.CardId))
        {
            _logger.LogWarning("Unauthorized attempt to update a logo for card with ID {TargetCardId}.", request.CardId);
            throw AuthorizationException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        var card = await _cardRepository.GetAsync(new FindCardWithLogoSpecification(request.CardId), cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);

        if (request.File is null)
        {
            card.SetHasLogo(false);
            card.Logo.SetContent(null);
            card.Logo.SetContentType(null);
        }
        else
        {
            var resizedFile = _imageService.FitImageToSize(FileInMemory.CreateFromFormFile(request.File), MaxIconWidth, MaxIconHeight);           

            card.SetHasLogo(true);
            card.Logo.SetContent(resizedFile.Data);
            card.Logo.SetContentType(resizedFile.ContentType);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return card.Id;
    }
}