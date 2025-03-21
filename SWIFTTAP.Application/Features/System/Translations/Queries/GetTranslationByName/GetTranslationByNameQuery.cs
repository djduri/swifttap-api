using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.System.Translations.DTOs;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetTranslationByName;
// Include properties to be used as input for the query
public sealed record GetTranslationByNameQuery(string Name) : IQuery<TranslationDetailsDTO>;
