using Microsoft.AspNetCore.Http;
using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.LinkIcons.Commands.UpdateLinkIcon;

// Include properties to be used as input for the command
public sealed record UpdateLinkIconCommand(IFormFile? File, long LinkId) : ICommand<long>;