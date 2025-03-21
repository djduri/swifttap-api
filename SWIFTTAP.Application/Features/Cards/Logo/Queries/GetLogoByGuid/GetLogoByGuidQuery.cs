using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Helpers;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByGuid;
// Include properties to be used as input for the query
public sealed record GetLogoByGuidQuery(Guid Guid) : IQuery<FileInMemory>;
