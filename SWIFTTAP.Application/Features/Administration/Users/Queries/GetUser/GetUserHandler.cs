using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetUser;
internal sealed class GetUserHandler : IQueryHandler<GetUserQuery, UserDetailsDTO>
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _dbContext;
    private readonly IUserService _userService;

    public GetUserHandler(IMapper mapper,
                          DatabaseContext dbContext,
                          IUserService userService)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<UserDetailsDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var loggedUserId = _userService.GetAuthenticatedUserId();

        var user = await _dbContext.Users.AsNoTracking()
                                         .Include(x => x.Card)
                                         .SingleOrDefaultAsync(x => x.Id == loggedUserId) ??
                    throw EntityNotFoundException.FromErrorCode(ErrorCodes.User.NotFound);

        var roleIds = await _dbContext.UserRoles
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleId)
            .ToListAsync(cancellationToken);

        var roles = await _dbContext.Roles
            .Where(x => roleIds.Contains(x.Id))
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<UserDetailsDTO>(user);
        result.Role = roles.FirstOrDefault()!;

        return result;
    }
}
