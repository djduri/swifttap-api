using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Helpers;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByUniqueName;
// Include properties to be used as input for the query
public sealed record GetLogoByUniqueNameQuery(string UniqueName) : IQuery<FileInMemory>;
