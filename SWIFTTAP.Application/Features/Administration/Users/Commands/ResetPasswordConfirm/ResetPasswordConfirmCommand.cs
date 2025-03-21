using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordConfirm;

public sealed record ResetPasswordConfirmCommand(
    string Email,
    string Token,
    string NewPassword) : ICommand<long>;