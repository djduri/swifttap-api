using SWIFTTAP.Domain.Base;

namespace SWIFTTAP.Infrastructure.Abstractions;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<IList<TEntity>?> GetManyAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);
    Task<IList<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<bool> AllExistAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
}
