using Microsoft.AspNetCore.Http;
using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Logo.Commands.UpdateLogo;
// Include properties to be used as input for the command
public sealed record UpdateLogoCommand(IFormFile? File, long CardId) : ICommand<long>;