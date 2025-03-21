using System.Linq.Expressions;
using SWIFTTAP.Domain.Base;

namespace SWIFTTAP.Infrastructure.Abstractions;

public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : class, IEntity
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; }
    public IList<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new List<Expression<Func<TEntity, object>>>();
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> expression) =>
        IncludeExpressions.Add(expression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> expression) =>
        OrderByExpression = expression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> expression) =>
        OrderByDescendingExpression = expression;
}
