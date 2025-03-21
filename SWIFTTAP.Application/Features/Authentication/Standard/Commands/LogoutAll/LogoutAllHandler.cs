using MediatR;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.LogoutAll;

internal sealed class LogoutAllHandler : ICommandHandler<LogoutAllCommand, Unit>
{
    private readonly IUserService _userService;
    private readonly IRepository<User> _userRepository;
    private readonly ILogger<LogoutAllHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutAllHandler(IUserService userService,
                            IRepository<User> userRepository,
                            ILogger<LogoutAllHandler> logger,
                            IUnitOfWork unitOfWork)
    {
        _userService = userService;
        _userRepository = userRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
    {
        var userName = _userService.GetAuthenticatedUserEmailOrDefault();

        if (string.IsNullOrEmpty(userName))
        {
            _logger.LogInformation("Logout attempt for user without email in claims");
            throw AuthorizationException.FromErrorCode(ErrorCodes.User.NotAuthenticated);
        }

        var user = await GetUserOrThrowAsync(userName, cancellationToken);

        // Reset two-factor key if applicable
        if (user.UserKeys?.AuthTwoFactorKey != null)
        {
            user.UserKeys.ResetAuthTwoFactorKey();
        }

        // Revoke all refresh tokens
        user.RevokeAllRefreshTokens();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    // Helper method to get the user or throw if not found
    private async Task<User> GetUserOrThrowAsync(string userName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithUserKeysAndNonRevokedRefreshTokensSpecification(userName), cancellationToken);
        if (user == null)
        {
            _logger.LogInformation("Logout attempt for non-existing user: {userName}", userName);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }
        return user;
    }
}
