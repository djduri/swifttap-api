using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.RefreshToken;
// Include properties to be used as input for the command
public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<TokenDTO>;