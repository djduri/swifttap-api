using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Authentication.Standard.DTOs;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.Authenticate;
// Include properties to be used as input for the command
public sealed record AuthenticateCommand(string Username, string Password) : ICommand<AuthenticateDTO>;