using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.IsUniqueNameAvailable;
// Include properties to be used as input for the query
public sealed record IsUniqueNameAvailableQuery(string UniqueName) : IQuery<bool>;
