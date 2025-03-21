using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Helpers;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogo;
// Include properties to be used as input for the query
public sealed record GetLogoQuery(long CardId) : IQuery<FileInMemory>;
