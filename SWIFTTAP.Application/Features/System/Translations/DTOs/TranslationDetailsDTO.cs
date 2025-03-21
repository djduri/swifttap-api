using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Features.System.Translations.DTOs;
public sealed class TranslationDetailsDTO
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public string? Content { get; set; }
    public required Language Language { get; set; }
}
