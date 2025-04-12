using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Application.Features.Cards.Links.Specifications;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
internal sealed class CreateLinkHandler : ICommandHandler<CreateLinkCommand, long>
{
    private readonly IRepository<Link> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public CreateLinkHandler(IRepository<Link> repository,
              IUnitOfWork unitOfWork,
              IUserService userService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<long> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var cardId = _userService.GetAuthenticatedUserCardId(); 

        var newLink = Link.Factory.Create(request.Name,
                                          request.Type,
                                          request.Url,
                                          request.Order,
                                          cardId);
        _repository.Add(newLink);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newLink.Id;
    }
}