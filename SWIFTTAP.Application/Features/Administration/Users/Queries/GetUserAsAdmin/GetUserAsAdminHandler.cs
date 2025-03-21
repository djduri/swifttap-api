using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.DTOs;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.GetUserAsAdmin;
internal sealed class GetUserAsAdminHandler : IQueryHandler<GetUserAsAdminQuery, UserDetailsDTO>
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _dbContext;

    public GetUserAsAdminHandler(IMapper mapper,
                                 DatabaseContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<UserDetailsDTO> Handle(GetUserAsAdminQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking()
                                         .Include(x => x.Card)
                                         .SingleOrDefaultAsync(x => x.Id == request.Id) ??
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
