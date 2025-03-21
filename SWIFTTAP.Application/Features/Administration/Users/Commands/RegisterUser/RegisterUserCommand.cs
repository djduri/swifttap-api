using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterUser;
// Include properties to be used as input for the command
public sealed record RegisterUserCommand(string Email, string Password, string Name, string UniqueName) : ICommand<long>;