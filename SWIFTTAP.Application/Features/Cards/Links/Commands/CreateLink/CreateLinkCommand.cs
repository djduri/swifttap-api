using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
// Include properties to be used as input for the command
public sealed record CreateLinkCommand(string Type, string Name, string Url, int Order) : ICommand<long>;