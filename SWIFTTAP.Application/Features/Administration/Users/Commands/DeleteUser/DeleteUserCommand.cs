using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.DeleteUser;
// Include properties to be used as input for the command
public sealed record DeleteUserCommand(long Id) : ICommand<long>;