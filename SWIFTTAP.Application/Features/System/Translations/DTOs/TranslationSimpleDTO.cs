using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Features.System.Translations.DTOs;
public sealed class TranslationSimpleDTO
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required Language Language { get; set; }
}
