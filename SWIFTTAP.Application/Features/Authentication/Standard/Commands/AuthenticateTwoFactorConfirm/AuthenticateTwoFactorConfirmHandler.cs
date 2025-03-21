using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.AuthenticateTwoFactorConfirm;
internal sealed class AuthenticateTwoFactorConfirmHandler : ICommandHandler<AuthenticateTwoFactorConfirmCommand, TokenDTO>
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticateTwoFactorConfirmHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;

    public AuthenticateTwoFactorConfirmHandler(ITokenService tokenService,
                                               ILogger<AuthenticateTwoFactorConfirmHandler> logger,
                                               IUnitOfWork unitOfWork,
                                               IRepository<User> userRepository)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<TokenDTO> Handle(AuthenticateTwoFactorConfirmCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithUserKeysSpecification(request.Username), cancellationToken) ??
                   throw LogAndThrow(request.Username, ErrorCodes.Authentication.Failed, "Non-existent user attempted two-factor confirmation");

        if (user.TwoFactorEnabled && IsTwoFactorCodeValid(user, request))
        {
            if (!user.UserKeys!.IsAuthTwoFactorCodeStillValid())            
                throw LogAndThrow(request.Username, ErrorCodes.Authentication.TwoFactorCodeExpired, "Two-factor code expired");            

            user.UserKeys.ResetAuthTwoFactorKey();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return await _tokenService.CreateTokenWithRefreshTokenAsync(user, cancellationToken);
        }

        throw LogAndThrow(request.Username, ErrorCodes.Authentication.Failed, "Invalid two-factor code");
    }

    private bool IsTwoFactorCodeValid(User user, AuthenticateTwoFactorConfirmCommand request)
    {
        return user.UserKeys != null &&
               user.UserKeys.AuthTwoFactorKey == request.AuthTwoFactorKey &&
               user.UserKeys.AuthTwoFactorCode == request.AuthTwoFactorCode;
    }

    private AuthenticationException LogAndThrow(string username, ErrorCodes.Authentication errorCode, string message)
    {
        _logger.LogInformation(message + " for user: {userName}", username);
        return AuthenticationException.FromErrorCode(errorCode);
    }
}
