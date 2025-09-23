using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
// Include properties to be used as input for the command
public sealed record CreateLinkCommand(long CardId, string Type, string Name, string Url, int Order, LinkKind LinkKind) : ICommand<long>;