using System.Linq.Expressions;
using SWIFTTAP.Domain.Base;

namespace SWIFTTAP.Infrastructure.Abstractions;

public interface ISpecification<TEntity> where TEntity : class, IEntity
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    IList<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
    Expression<Func<TEntity, object>>? OrderByExpression { get; }
    Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; }
}
