using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLinkAsAdmin;
// Include properties to be used as input for the query
public sealed record GetNfcLinkAsAdminQuery(long CardId) : IQuery<string>;
