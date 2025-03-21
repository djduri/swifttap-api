using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.DeleteTranslation;
// Include properties to be used as input for the command
public sealed record DeleteTranslationCommand(long Id) : ICommand<long>;