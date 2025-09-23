using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Links.Specifications;
using SWIFTTAP.Application.Helpers;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.LinkIcons.Commands.UpdateLinkIcon;

internal sealed class UpdateLinkIconHandler : ICommandHandler<UpdateLinkIconCommand, long>
{
    private const int MaxIconWidth = 250;
    private const int MaxIconHeight = 250;

    private readonly IRepository<Link> _linkRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IUserService _userService;
    private readonly ILogger<UpdateLinkIconHandler> _logger;
    private readonly DatabaseContext _dbContext;

    public UpdateLinkIconHandler(IRepository<Link> linkRepository,
                             IUnitOfWork unitOfWork,
                             IImageService imageService,
                             IUserService userService,
                             ILogger<UpdateLinkIconHandler> logger,
                             DatabaseContext dbContext)
    {
        _linkRepository = linkRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _userService = userService;
        _logger = logger;
        _dbContext = dbContext;
    }


    public async Task<long> Handle(UpdateLinkIconCommand request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId();

        var link = await _linkRepository.GetAsync(new FindLinkWithLinkIconSpecification(request.LinkId), cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Link.NotFound);

        // Sprawdzamy, czy użytkownik jest administratorem lub jest właścicielem linku
        if (!_userService.IsAuthenticatedUserAdmin() && cardId != link.CardId)
        {
            // Dodanie logowania, gdy użytkownik nie jest administratorem i próbuje zmienić link, który nie należy do niego
            _logger.LogWarning("A user tried to update a link icon with ID {LinkId} that does not belong to them. Access denied.", link.Id);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        if (link.LinkKind != LinkKind.Custom)
        {
            _logger.LogWarning("Attempt to update icon for a link with ID {LinkId} that is not of kind 'Custom'.", link.Id);
            throw EntityUpdateException.FromErrorCode(ErrorCodes.LinkIcon.InvalidLinkKind);
        }

        if (request.File is null)
        {
            link.SetHasIcon(false);

            if (link.LinkIcon is not null)
                _dbContext.LinkIcons.Remove(link.LinkIcon);
        }
        else
        {
            var resizedFile = _imageService.FitImageToSize(FileInMemory.CreateFromFormFile(request.File), MaxIconWidth, MaxIconHeight);

            link.SetHasIcon(true);

            if (link.LinkIcon is null)
            {
                var linkIcon = LinkIcon.Factory.FromArguments(link, resizedFile.Data, resizedFile.ContentType);                
                _dbContext.LinkIcons.Add(linkIcon);
            }
            else
            {
                link.LinkIcon.SetContent(resizedFile.Data);
                link.LinkIcon.SetContentType(resizedFile.ContentType);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return link.Id;
    }
}