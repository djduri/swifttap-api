using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(string Name, string UniqueName, bool TwoFactorEnabled) : ICommand<long>;