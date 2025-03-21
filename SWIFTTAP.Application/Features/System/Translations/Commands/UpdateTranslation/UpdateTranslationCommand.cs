using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.UpdateTranslation;
// Include properties to be used as input for the command
public sealed record UpdateTranslationCommand(long Id,
                                              string Name,
                                              Language? Language,
                                              string? Content) : ICommand<long>;