using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordAsAdmin;
// Include properties to be used as input for the command
public sealed record ResetPasswordAsAdminCommand(long Id) : ICommand<long>;