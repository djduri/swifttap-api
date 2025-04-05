using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.IsUniqueNameAvailable;
internal sealed class IsUniqueNameAvailableHandler : IQueryHandler<IsUniqueNameAvailableQuery, bool>
{
    private readonly DatabaseContext _dbContext;

    public IsUniqueNameAvailableHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(IsUniqueNameAvailableQuery request, CancellationToken cancellationToken)
    {
        var taken = await _dbContext.Cards.AnyAsync(x => x.UniqueName == request.UniqueName, cancellationToken);

        return !taken;
    }
}
