using System.Linq.Expressions;
using SWIFTTAP.Domain.Base;

namespace SWIFTTAP.Infrastructure.Abstractions;

public interface IUnitOfWork
{
    int SaveChanges();
    int SaveChanges(bool skipAuditableSubjectUpdate);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(bool skipAuditableSubjectUpdate, CancellationToken cancellationToken = default);
    ITransaction BeginTransaction();
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel,
                                             CancellationToken cancellationToken = default);

    Task IncludeToEntity<T, TProperty>(T entity, Expression<Func<T, TProperty>> navigationProperty) where T : class, IEntity;
}
