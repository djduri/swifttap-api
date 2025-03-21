using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.System.Translations.DTOs;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetAllTranslations;
// Include properties to be used as input for the query
public sealed record GetAllTranslationsQuery(string? Filter,
                                             Language? Language,
                                             SortingArguments? SortingArguments,
                                             PaginationArguments? PaginationArguments) : IQuery<RangedDTO<TranslationSimpleDTO>>;
