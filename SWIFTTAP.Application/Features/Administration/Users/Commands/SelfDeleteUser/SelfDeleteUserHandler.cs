using Microsoft.AspNetCore.Identity;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.SelfDeleteUser;
internal sealed class SelfDeleteUserHandler : ICommandHandler<SelfDeleteUserCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<DeletedUser> _deletedUserRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public SelfDeleteUserHandler(UserManager<User> userManager,
                                 IRepository<DeletedUser> deletedUserRepository,
                                 IUserService userService,
                                 IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _deletedUserRepository = deletedUserRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(SelfDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var emailFromClaims = _userService.GetAuthenticatedUserEmailOrDefault();

        if (emailFromClaims != request.Email)
            throw EntityDeleteException.FromErrorCode(ErrorCodes.Application.AccessDenied);

        var user = await _userManager.FindByEmailAsync(request.Email) ??
                   throw EntityDeleteException.FromErrorCode(ErrorCodes.User.NotFound);

        if (_userService.IsUserSuperAdmin(user))
            throw EntityDeleteException.FromErrorCode(ErrorCodes.Application.AccessDenied);

        var deletedUser = DeletedUser.Factory.Create(user.Name,
                                                     request.Email,
                                                     request.Reason);

        _deletedUserRepository.Add(deletedUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _userManager.DeleteAsync(user);

        return deletedUser.Id;
    }
}