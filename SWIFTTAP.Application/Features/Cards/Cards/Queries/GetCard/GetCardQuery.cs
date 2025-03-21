using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCard;
// Include properties to be used as input for the query
public sealed record GetCardQuery(Guid Guid) : IQuery<string>;
