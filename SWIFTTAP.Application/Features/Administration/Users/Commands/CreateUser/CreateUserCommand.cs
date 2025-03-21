using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.CreateUser;
// Include properties to be used as input for the command
public sealed record CreateUserCommand(string Email, string Name, string UniqueName) : ICommand<long>;