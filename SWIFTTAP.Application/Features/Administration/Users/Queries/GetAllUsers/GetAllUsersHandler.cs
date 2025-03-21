using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetAllUsers;

internal sealed class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery, RangedDTO<UserSimpleDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(DatabaseContext dbContext, UserManager<User> userManager, IMapper mapper)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<RangedDTO<UserSimpleDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = _dbContext.Users.AsNoTracking();

        var userRoles = await _dbContext.UserRoles.AsNoTracking().ToListAsync(cancellationToken);
        var roles = await _dbContext.Roles.AsNoTracking().ToListAsync(cancellationToken);

        if (request.Role is not null)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role.ToString());
            usersQuery = usersQuery.Where(u => usersInRole.Select(ur => ur.Id).Contains(u.Id));
        }

        if (request.CreatedAtFrom is not null)        
            usersQuery = usersQuery.Where(u => u.CreatedAt >= request.CreatedAtFrom.Value);

        if (request.CreatedAtTo is not null)
            usersQuery = usersQuery.Where(u => u.CreatedAt <= request.CreatedAtTo.Value);


        if (request.Filter is not null)
        {
            var filterToLower = request.Filter.ToLower();
            usersQuery = usersQuery.Where(x => x.Email!.ToLower().Contains(filterToLower)
                                               || x.Name.ToLower().Contains(filterToLower));
        }          

        if (request.SortingArguments is not null)
        {
            usersQuery = usersQuery
                .Sort(request.SortingArguments.SortBy, request.SortingArguments.Desc)
                .With(x => x.Name)
                .With(x => x.Email)
                .With(x => x.CreatedAt)
                .AsQueryable();
        }

        var users = await usersQuery.ToPagedListAsync(request.PaginationArguments, cancellationToken);

        var mappedUsers = _mapper.Map<IEnumerable<UserSimpleDTO>>(users);
        AttachRoleToUser(userRoles, roles, mappedUsers);

        return new RangedDTO<UserSimpleDTO>(mappedUsers, users.TotalCount);
    }

    private static void AttachRoleToUser(List<IdentityUserRole<long>> userRoles,
                                         List<IdentityRole<long>> roles,
                                         IEnumerable<UserSimpleDTO> usersResult)
    {
        foreach (var user in usersResult)
        {
            var rolesId = userRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
            user.Role = roles.Where(x => rolesId.Contains(x.Id)).Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name!).FirstOrDefault();                     
        }
    }
}
