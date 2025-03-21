using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Cards.Links.DTOs;

namespace SWIFTTAP.Application.Features.Cards.Links.Queries.GetLink;
// Include properties to be used as input for the query
public sealed record GetLinkQuery(long Id) : IQuery<LinkDetailsDTO>;
