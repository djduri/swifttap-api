using Microsoft.AspNetCore.Identity;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.DeleteUser;
internal sealed class DeleteUserHandler : ICommandHandler<DeleteUserCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;

    public DeleteUserHandler(UserManager<User> userManager,
                             IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    public async Task<long> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString()) ??
                   throw EntityDeleteException.FromErrorCode(ErrorCodes.User.NotFound);

        if (_userService.IsUserSuperAdmin(user))
            throw EntityDeleteException.FromErrorCode(ErrorCodes.Application.AccessDenied);

        await _userManager.DeleteAsync(user);

        return request.Id;
    }
}