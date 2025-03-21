using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.DeleteLink;
// Include properties to be used as input for the command
public sealed record DeleteLinkCommand(long Id) : ICommand<long>;