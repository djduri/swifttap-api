using SWIFTTAP.Application.Abstractions;
using MediatR;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.Logout;
// Include properties to be used as input for the command
public sealed record LogoutCommand(string RefreshToken) : ICommand<Unit>;