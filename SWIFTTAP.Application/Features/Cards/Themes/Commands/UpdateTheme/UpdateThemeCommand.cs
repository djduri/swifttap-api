using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Cards.Themes.Commands.UpdateTheme;
// Include properties to be used as input for the command
public sealed record UpdateThemeCommand(string? Name,
                                        string? PrimaryColor,
                                        string? SecondaryColor,
                                        string? TextDark,
                                        string? TextLight,
                                        string? Background,
                                        string? TextOnButtons,
                                        string? LinkBackgroundColor,
                                        string? LinkTextColor,
                                        bool HasSharpEdges,
                                        bool RoundedProfilePicture) : ICommand<long>;
