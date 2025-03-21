using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Authorization;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(string? Filter,
                                      DateTime? CreatedAtFrom,
                                      DateTime? CreatedAtTo,
                                      Roles? Role,
                                      SortingArguments? SortingArguments,
                                      PaginationArguments? PaginationArguments) : IQuery<RangedDTO<UserSimpleDTO>>;
