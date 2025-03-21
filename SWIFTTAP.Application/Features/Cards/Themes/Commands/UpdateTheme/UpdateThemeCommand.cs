using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Cards.Themes.Commands.UpdateTheme;
// Include properties to be used as input for the command
public sealed record UpdateThemeCommand(string? Name, string? PrimaryColor, string? SecondaryColor, bool HasSharpEdges) : ICommand<long>;
