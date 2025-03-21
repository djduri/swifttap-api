using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.CreateAndSendTwoFactorCode;
// Include properties to be used as input for the command
public sealed record CreateAndSendTwoFactorCodeCommand(string Username, string AuthTwoFactorKey) : ICommand<string>;