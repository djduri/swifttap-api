using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Cards.Cards.DTOs;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCardByUniqueName;
// Include properties to be used as input for the query
public sealed record GetCardByUniqueNameQuery(string UniqueName) : IQuery<CardDetailsDTO>;
