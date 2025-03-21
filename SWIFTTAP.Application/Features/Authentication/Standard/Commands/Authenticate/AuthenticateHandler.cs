using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Authentication.Standard.DTOs;
using SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.Authenticate;

internal sealed class AuthenticateHandler : ICommandHandler<AuthenticateCommand, AuthenticateDTO>
{
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticateHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IUserEmailService _userEmailService;
    private readonly AuthenticationSettings _authenticationSettings;

    public AuthenticateHandler(
        SignInManager<User> signInManager,
        ITokenService tokenService,
        ILogger<AuthenticateHandler> logger,
        IUnitOfWork unitOfWork,
        IRepository<User> userRepository,
        IUserEmailService userEmailService,
        IOptions<AuthenticationSettings> authenticationSettings)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userEmailService = userEmailService;
        _authenticationSettings = authenticationSettings.Value;
    }

    public async Task<AuthenticateDTO> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithUserKeysSpecification(request.Username), cancellationToken)
                    ?? throw LogAndThrowAuthenticationException(request.Username,
                                                                ErrorCodes.Authentication.Failed,
                                                                "non existing user");

        var signInResult = await _signInManager.PasswordSignInAsync(user,
                                                                    request.Password,
                                                                    false,
                                                                    true);

        if (signInResult.Succeeded)
        {
            var tokenDTO = await _tokenService.CreateTokenWithRefreshTokenAsync(user, cancellationToken);
            return new AuthenticateDTO(tokenDTO);
        }

        if (signInResult.IsLockedOut)
        {
            var lockoutEnd = user.LockoutEnd?.ToString("dd-MM-yyyy HH:mm:ss") ?? "unknown";
            throw LogAndThrowAuthenticationException(
                request.Username,
                ErrorCodes.Authentication.LockedOut,
                $"locked-out user lockout end: {lockoutEnd}");
        }                

        if (signInResult.RequiresTwoFactor && user.UserKeys is not null)
        {
            await HandleTwoFactorAuthentication(user, cancellationToken);
            return new AuthenticateDTO(user.UserKeys.AuthTwoFactorKey);
        }

        throw LogAndThrowAuthenticationException(request.Username, ErrorCodes.Authentication.Failed, "Wrong password");
    }

    private async Task HandleTwoFactorAuthentication(User user, CancellationToken cancellationToken)
    {
        user.UserKeys!.SetAuthTwoFactorKey(_authenticationSettings.TwoFactorCodeExpirationMinutes);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (!await _userEmailService.Send2FaAuthenticationCodeAsync(user))        
            throw AuthenticationException.FromErrorCode(ErrorCodes.Authentication.Failed);        
    }

    private AuthenticationException LogAndThrowAuthenticationException(string username, ErrorCodes.Authentication errorCode, string message)
    {
        _logger.LogInformation("Authentication of user: {userName} failed. {message}", username, message);
        return AuthenticationException.FromErrorCode(errorCode);
    }
}
