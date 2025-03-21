using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Database;
using MediatR;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.Logout;

internal sealed class LogoutHandler : ICommandHandler<LogoutCommand, Unit>
{
    private readonly IUserService _userService;
    private readonly IRepository<User> _userRepository;
    private readonly ILogger<LogoutHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _databaseContext;

    public LogoutHandler(IUserService userService,
                         IRepository<User> userRepository,
                         ILogger<LogoutHandler> logger,
                         IUnitOfWork unitOfWork,
                         DatabaseContext databaseContext)
    {
        _userService = userService;
        _userRepository = userRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _databaseContext = databaseContext;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userName = _userService.GetAuthenticatedUserEmailOrDefault();
        if (string.IsNullOrEmpty(userName))
        {
            _logger.LogInformation("Logout attempt for user without email in claims");
            throw AuthorizationException.FromErrorCode(ErrorCodes.User.NotAuthenticated);
        }

        var user = await GetUserOrThrowAsync(userName, cancellationToken);

        var needToSaveChanges = false;

        // Reset two-factor key if applicable
        if (user.UserKeys?.AuthTwoFactorKey != null)
        {
            user.UserKeys.ResetAuthTwoFactorKey();
            needToSaveChanges = true;
        }

        // Revoke the refresh token if valid
        var refreshToken = await _databaseContext.RefreshTokens.FirstOrDefaultAsync(x => x.Value == request.RefreshToken, cancellationToken);

        if (refreshToken != null)
        {
            refreshToken.Revoke();
            needToSaveChanges = true;
        }

        // Save changes to the database only if needed
        if (needToSaveChanges)        
            await _unitOfWork.SaveChangesAsync(cancellationToken);        

        return Unit.Value;
    }

    // Helper method to get the user or throw if not found
    private async Task<User> GetUserOrThrowAsync(string userName, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithUserKeysSpecification(userName), cancellationToken);
        if (user == null)
        {
            _logger.LogInformation("Logout attempt for non-existing user: {userName}", userName);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }
        return user;
    }
}
