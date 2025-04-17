using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Features.Administration.DeletedUsers.DTOs;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.Queries.GetAllDeletedUsers;
// Include properties to be used as input for the query
public sealed record GetAllDeletedUsersQuery(string? Filter,
                                             DateTime? CreatedAtFrom,
                                             DateTime? CreatedAtTo,                                     
                                             SortingArguments? SortingArguments,
                                             PaginationArguments? PaginationArguments) : IQuery<RangedDTO<DeletedUserDTO>>;
