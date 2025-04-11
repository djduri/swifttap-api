using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.UpdateLink;
// Include properties to be used as input for the command
public sealed record UpdateLinkCommand(long Id, string Type, string Name, string Url, int Order) : ICommand<long>;