using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetUser;
// Include properties to be used as input for the query
public sealed record GetUserQuery() : IQuery<UserDetailsDTO>;
