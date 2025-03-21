using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Extensions;

namespace SWIFTTAP.Infrastructure.Database;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _databaseContext;

    public UnitOfWork(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext ??
            throw new ArgumentNullException(nameof(databaseContext));
    }

    public ITransaction BeginTransaction()
    {
        return Transaction.Create(_databaseContext);
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Transaction.CreateAsync(_databaseContext);
    }

    public async Task<ITransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel,
                                                          CancellationToken cancellationToken = default)
    {
        return await Transaction.CreateAsync(_databaseContext, isolationLevel, cancellationToken);
    }

    public int SaveChanges()
    {
        _databaseContext.ChangeAuditableSubjectUpdateBehaviour(false);

        return _databaseContext.SaveChanges();
    }

    public int SaveChanges(bool skipAuditableSubjectUpdate)
    {
        _databaseContext.ChangeAuditableSubjectUpdateBehaviour(!skipAuditableSubjectUpdate);

        return _databaseContext.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(false, cancellationToken);
    }

    public Task<int> SaveChangesAsync(bool skipAuditableSubjectUpdate, CancellationToken cancellationToken = default)
    {
        _databaseContext.ChangeAuditableSubjectUpdateBehaviour(!skipAuditableSubjectUpdate);

        return _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task IncludeToEntity<T, TProperty>(T entity, Expression<Func<T, TProperty>> navigationProperty)
        where T : class, IEntity
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (navigationProperty == null)
            throw new ArgumentNullException(nameof(navigationProperty));


        var entry = _databaseContext.Entry(entity);

        if (navigationProperty.Body is MemberExpression || navigationProperty.Body is MethodCallExpression)
        {
            await entry.Include(navigationProperty).LoadAsync();
        }
        else
        {
            throw new ArgumentException("Invalid navigation property expression", nameof(navigationProperty));
        }
    }
}
