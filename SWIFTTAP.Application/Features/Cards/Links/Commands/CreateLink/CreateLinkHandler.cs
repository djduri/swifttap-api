using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
internal sealed class CreateLinkHandler : ICommandHandler<CreateLinkCommand, long>
{
    private readonly IRepository<Link> _repository;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly ILogger<CreateLinkHandler> _logger;

    public CreateLinkHandler(IRepository<Link> repository,
                             IRepository<User> userRepository,
                             IUnitOfWork unitOfWork,
                             IUserService userService,
                             ILogger<CreateLinkHandler> logger)
    {
        _repository = repository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _logger = logger;
    }

    public async Task<long> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        if (!_userService.HasAuthUserPermissionToCard(request.CardId))
        {
            _logger.LogWarning("Unauthorized attempt to create a link for card with ID {TargetCardId}.", request.CardId);
            throw AuthorizationException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        var newLink = Link.Factory.Create(request.Name,
                                          request.Type,
                                          request.Url,
                                          request.Order,
                                          request.CardId,
                                          request.LinkKind);
        _repository.Add(newLink);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newLink.Id;
    }
}