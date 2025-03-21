using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.AuthenticateTwoFactorConfirm;
// Include properties to be used as input for the command
public sealed record AuthenticateTwoFactorConfirmCommand(string Username, string AuthTwoFactorKey, string AuthTwoFactorCode) : ICommand<TokenDTO>;