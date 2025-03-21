using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.CreateAdmin;
// Include properties to be used as input for the command
public sealed record CreateAdminCommand(string Email, string Name, string UniqueName) : ICommand<long>;