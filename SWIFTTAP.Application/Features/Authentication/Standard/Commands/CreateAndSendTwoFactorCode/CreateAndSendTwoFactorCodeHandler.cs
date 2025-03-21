using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Authentication.Standard.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.CreateAndSendTwoFactorCode;

internal sealed class CreateAndSendTwoFactorCodeHandler : ICommandHandler<CreateAndSendTwoFactorCodeCommand, string>
{
    private readonly ILogger<CreateAndSendTwoFactorCodeHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IUserEmailService _userEmailService;
    private readonly AuthenticationSettings _authenticationSettings;

    public CreateAndSendTwoFactorCodeHandler(ILogger<CreateAndSendTwoFactorCodeHandler> logger,
                                             IUnitOfWork unitOfWork,
                                             IRepository<User> userRepository,
                                             IUserEmailService userEmailService,
                                             IOptions<AuthenticationSettings> authenticationSettings)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationSettings = authenticationSettings.Value;
        _userEmailService = userEmailService;
    }

    public async Task<string> Handle(CreateAndSendTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        // Fetch user based on the request username
        var user = await _userRepository.GetAsync(new FindUserWithUserKeysSpecification(request.Username), cancellationToken);
        if (user is null)
        {
            _logger.LogInformation("Attempted to create and send 2FA code for non-existent user: {userName}", request.Username);
            throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        // Validate the two-factor key and proceed with code generation
        if (IsTwoFactorValid(user, request.AuthTwoFactorKey))
        {
            user.UserKeys!.SetAuthTwoFactorKey(_authenticationSettings.TwoFactorCodeExpirationMinutes);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send the email with the new 2FA code
            if (!await _userEmailService.Send2FaAuthenticationCodeAsync(user))
                throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);

            return user.UserKeys.AuthTwoFactorKey!;
        }

        _logger.LogInformation("Failed to send 2FA code for user: {userName}", request.Username);
        throw AccessDeniedException.FromErrorCode(ErrorCodes.Application.AccessDenied);
    }

    // Helper method to check the validity of the 2FA key
    private bool IsTwoFactorValid(User user, string authTwoFactorKey)
    {
        return user.TwoFactorEnabled && user.UserKeys?.AuthTwoFactorKey == authTwoFactorKey;
    }
}
