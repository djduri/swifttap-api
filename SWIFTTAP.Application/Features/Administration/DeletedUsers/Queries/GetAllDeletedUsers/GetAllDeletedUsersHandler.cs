using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Features.Administration.DeletedUsers.DTOs;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.Queries.GetAllDeletedUsers;
internal sealed class GetAllDeletedUsersHandler : IQueryHandler<GetAllDeletedUsersQuery, RangedDTO<DeletedUserDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllDeletedUsersHandler(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<RangedDTO<DeletedUserDTO>> Handle(GetAllDeletedUsersQuery request, CancellationToken cancellationToken)
    {
        var deletedUsersQuery = _dbContext.DeletedUsers.AsNoTracking();       

        if (request.CreatedAtFrom is not null)
            deletedUsersQuery = deletedUsersQuery.Where(u => u.CreatedAt >= request.CreatedAtFrom.Value);

        if (request.CreatedAtTo is not null)
            deletedUsersQuery = deletedUsersQuery.Where(u => u.CreatedAt <= request.CreatedAtTo.Value);


        if (request.Filter is not null)
        {
            var filterToLower = request.Filter.ToLower();
            deletedUsersQuery = deletedUsersQuery.Where(x => x.Email!.ToLower().Contains(filterToLower)
                                                             || x.Name.ToLower().Contains(filterToLower));
        }

        if (request.SortingArguments is not null)
        {
            deletedUsersQuery = deletedUsersQuery
                .Sort(request.SortingArguments.SortBy, request.SortingArguments.Desc)
                .With(x => x.Name)
                .With(x => x.Email)
                .With(x => x.Reason)
                .With(x => x.CreatedAt)
                .AsQueryable();
        }

        var deletedUsers = await deletedUsersQuery.ToPagedListAsync(request.PaginationArguments, cancellationToken);

        return new RangedDTO<DeletedUserDTO>(_mapper.Map<IEnumerable<DeletedUserDTO>>(deletedUsers), deletedUsers.TotalCount);
    }    
}
