using SWIFTTAP.Application.Abstractions;
using MediatR;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.LogoutAll;
// Include properties to be used as input for the command
public sealed record LogoutAllCommand() : ICommand<Unit>;