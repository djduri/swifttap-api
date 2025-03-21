using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetNfcLink;
// Include properties to be used as input for the query
public sealed record GetNfcLinkQuery() : IQuery<string>;
