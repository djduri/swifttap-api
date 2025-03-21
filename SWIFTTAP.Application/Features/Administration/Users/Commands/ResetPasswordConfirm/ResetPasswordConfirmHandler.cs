using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordConfirm;

internal sealed class ResetPasswordConfirmHandler : ICommandHandler<ResetPasswordConfirmCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ResetPasswordConfirmHandler> _logger;

    public ResetPasswordConfirmHandler(UserManager<User> userManager, ILogger<ResetPasswordConfirmHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<long> Handle(ResetPasswordConfirmCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ??
            throw EntityUpdateException.FromErrorCode(ErrorCodes.Application.AccessDenied);

        var result = await _userManager.ResetPasswordAsync(
            user,
            request.Token.DecodeFromBase64(),
            request.NewPassword);

        if (result.Succeeded)
            return user.Id;

        var errors = result.Errors.Select(x => x.Description).ToArray();

        _logger.LogError("Failed to reset the password for {Email} due to {@Errors}",
            request.Email,
            errors);

        throw EntityUpdateException.FromErrorCode(ErrorCodes.User.CannotChangePassword);
    }
}