using System.Linq.Expressions;

namespace SWIFTTAP.Application.Extensions;

public interface ISortExpressionBuilder<TSource>
{
    ISortExpressionBuilder<TSource> With(Expression<Func<TSource, object?>> expression);
    ISortExpressionBuilder<TSource> With(params Expression<Func<TSource, object?>>[] expressions);
    ISortExpressionBuilder<TSource> With(Expression<Func<TSource, object?>> expression, string propertyName);
    ISortExpressionBuilder<TSource> By(string sortBy, bool descending);
    IQueryable<TSource> AsQueryable();
}

internal sealed class SortExpressionBuilder<TSource> : ISortExpressionBuilder<TSource>
{
    private readonly IQueryable<TSource> _collection;
    private readonly IDictionary<string, Expression<Func<TSource, object?>>> _expressions;
    private readonly IList<(string, bool)> _sortByValues;

    public SortExpressionBuilder(IQueryable<TSource> collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        _expressions = new Dictionary<string, Expression<Func<TSource, object?>>>();
        _sortByValues = new List<(string, bool)>();
    }

    public ISortExpressionBuilder<TSource> With(Expression<Func<TSource, object?>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return With(expression, SelectExpressionMemberName(expression));
    }

    public ISortExpressionBuilder<TSource> With(params Expression<Func<TSource, object?>>[] expressions)
    {
        foreach (var expression in expressions)
        {
            ArgumentNullException.ThrowIfNull(expression);
            With(expression);
        }

        return this;
    }

    public ISortExpressionBuilder<TSource> With(Expression<Func<TSource, object?>> expression, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        if (_expressions.ContainsKey(propertyName))
        {
            throw new ArgumentException($"Property name '{propertyName}' already exists in the sort expressions.", nameof(propertyName));
        }

        _expressions[propertyName] = expression;

        return this;
    }

    public ISortExpressionBuilder<TSource> By(string sortBy, bool descending = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sortBy);

        _sortByValues.Add((sortBy, descending));

        return this;
    }

    public IQueryable<TSource> AsQueryable()
    {
        if (_sortByValues.Count == 0) return _collection;

        var (firstSortBy, firstDescending) = _sortByValues.First();

        var firstOrderByExpression = _expressions[firstSortBy];

        var query = (firstDescending)
            ? _collection.OrderByDescending(firstOrderByExpression)
            : _collection.OrderBy(firstOrderByExpression);

        foreach (var (sortBy, descending) in _sortByValues.Skip(1))
        {
            var orderByExpression = _expressions[sortBy];

            query = (descending)
                ? query.ThenByDescending(orderByExpression)
                : query.ThenBy(orderByExpression);
        }

        return query;
    }

    private static IQueryable<TSource> ApplyOrdering(IQueryable<TSource> collection, (string, bool) sortByValue, IDictionary<string, Expression<Func<TSource, object?>>> expressions)
    {
        var (firstSortBy, firstDescending) = sortByValue;
        var firstOrderByExpression = expressions[firstSortBy];

        return firstDescending
            ? collection.OrderByDescending(firstOrderByExpression)
            : collection.OrderBy(firstOrderByExpression);
    }

    private static string SelectExpressionMemberName<TMember>(Expression<Func<TSource, TMember>> expression)
    {
        var memberExpression = (expression.Body as UnaryExpression)?.Operand as MemberExpression ??
            (expression.Body as MemberExpression);

        if (memberExpression == null)
            throw new ArgumentException("Failed to access the Source member via the expression", nameof(expression));

        return memberExpression.Member.Name;
    }
}
