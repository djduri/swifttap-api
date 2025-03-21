using Microsoft.AspNetCore.Identity;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ChangePassword;
internal sealed class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordHandler(UserManager<User> userManager,
                                 IUserService userService,
                                 IRepository<User> userRepository,
                                 IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _userService = userService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithNonRevokedRefreshTokensSpecification(_userService.GetAuthenticatedUserId()), cancellationToken) ??
                    throw EntityUpdateException.FromErrorCode(ErrorCodes.User.NotAuthenticated);

        var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);

        if (!result.Succeeded)
            throw EntityUpdateException.FromErrorCode(ErrorCodes.User.CannotChangePassword);

        // revoke all refresh tokens
        user.RevokeAllRefreshTokens();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}