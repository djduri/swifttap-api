using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.SelfDeleteUser;
// Include properties to be used as input for the command
public sealed record SelfDeleteUserCommand(string Email, string Reason) : ICommand<long>;