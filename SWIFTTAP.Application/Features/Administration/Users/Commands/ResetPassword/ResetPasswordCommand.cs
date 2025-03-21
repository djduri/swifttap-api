using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPassword;
// Include properties to be used as input for the command
public sealed record ResetPasswordCommand(string Email) : ICommand<long>;