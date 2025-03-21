using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.CreateTranslation;
// Include properties to be used as input for the command
public sealed record CreateTranslationCommand(string Name,
                                              Language? Language) : ICommand<long>;