using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Infrastructure.Database;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DatabaseContext _databaseContext;

    public Repository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    public void Add(TEntity entity) =>
        _databaseContext.Set<TEntity>().Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) =>
        _databaseContext.Set<TEntity>().AddRange(entities);

    public async Task<bool> AllExistAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default) =>
        await CheckIfEntitiesExist(ids.Distinct(), cancellationToken);

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        await _databaseContext.Set<TEntity>().AnyAsync(cancellationToken);

    public async Task<bool> AnyAsync(long id, CancellationToken cancellationToken = default) =>
        await _databaseContext.Set<TEntity>().AnyAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) =>
        await ExecuteSpecificationQuery(specification, query => query.AnyAsync(cancellationToken));

    public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        await _databaseContext.Set<TEntity>().CountAsync(cancellationToken);

    public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) =>
        await ExecuteSpecificationQuery(specification, query => query.CountAsync(cancellationToken));

    public void Delete(TEntity entity) =>
        _databaseContext.Set<TEntity>().Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities) =>
        _databaseContext.Set<TEntity>().RemoveRange(entities);

    public async Task<TEntity?> GetAsync(long id, CancellationToken cancellationToken = default) =>
        await _databaseContext.Set<TEntity>().SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public async Task<TEntity?> GetAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) =>
        await ExecuteSpecificationQuery(specification, query => query.SingleOrDefaultAsync(cancellationToken));

    public async Task<IList<TEntity>?> GetManyAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default) =>
        await GetEntitiesByIdsAsync(ids.Distinct(), cancellationToken);

    public async Task<IList<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) =>
        await ExecuteSpecificationQuery(specification, query => query.ToListAsync(cancellationToken));

    // Helper methods to simplify repeated logic:

    private async Task<bool> CheckIfEntitiesExist(IEnumerable<long> ids, CancellationToken cancellationToken) =>
        (await GetEntitiesByIdsAsync(ids, cancellationToken)).Count == ids.Count();

    private async Task<IList<TEntity>> GetEntitiesByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken) =>
        await _databaseContext.Set<TEntity>()
            .Where(entity => ids.Contains(entity.Id))
            .ToListAsync(cancellationToken);

    private async Task<TResult> ExecuteSpecificationQuery<TResult>(
        ISpecification<TEntity> specification,
        Func<IQueryable<TEntity>, Task<TResult>> queryExecution)
    {
        var query = SpecificationEvaluator.GetQuery(_databaseContext.Set<TEntity>(), specification);
        return await queryExecution(query);
    }
}
