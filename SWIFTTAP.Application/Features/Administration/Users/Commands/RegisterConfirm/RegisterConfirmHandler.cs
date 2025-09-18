using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterConfirm;

internal sealed class RegisterConfirmHandler : ICommandHandler<RegisterConfirmCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterConfirmHandler> _logger;

    public RegisterConfirmHandler(UserManager<User> userManager, ILogger<RegisterConfirmHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<long> Handle(RegisterConfirmCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ??
            throw EntityUpdateException.FromErrorCode(ErrorCodes.Application.AccessDenied);

        var result = await _userManager.ConfirmEmailAsync(user, request.Token.DecodeFromBase64());

        if (result.Succeeded)
            return user.Id;

        var errors = result.Errors.Select(x => x.Description).ToArray();

        _logger.LogError("Failed to confirm email for user Id {UserId}, Email {Email}. Errors: {@Errors}", user.Id, request.Email, errors);

        throw EntityUpdateException.FromErrorCode(ErrorCodes.User.CannotConfirmEmail);
    }
}
