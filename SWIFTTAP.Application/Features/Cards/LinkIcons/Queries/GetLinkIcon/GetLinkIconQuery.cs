using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Helpers;

namespace SWIFTTAP.Application.Features.Cards.LinkIcons.Queries.GetLinkIcon;

// Include properties to be used as input for the query
public sealed record GetLinkIconQuery(long LinkId) : IQuery<FileInMemory>;
