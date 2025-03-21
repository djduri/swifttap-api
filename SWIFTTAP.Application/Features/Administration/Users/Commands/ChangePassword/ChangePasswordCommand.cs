using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ChangePassword;
// Include properties to be used as input for the command
public sealed record ChangePasswordCommand(string Password,
                                           string NewPassword) : ICommand<long>;