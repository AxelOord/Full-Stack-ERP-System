using Domain.Primitives.Interfaces;
using Shared.Utils;
using System.Linq.Expressions;

namespace Shared.Specifications;

public class GetAllEntitiesSpecification<TEntity> : Specification<TEntity> where TEntity : IEntity
{
    public GetAllEntitiesSpecification(
        GetAllQueryParameters queryParameters)
    {
        if (queryParameters.RawFilters.Count > 0)
        {
            var filterDefinition = BracketNotationParser.ParseQueryString(queryParameters.RawFilters);
            var filterExpression = FilterExpressionBuilder.BuildFilterExpression<TEntity>(filterDefinition);
            AddCriteria(filterExpression);
        }

        // Apply filtering
        if (!string.IsNullOrEmpty(queryParameters.FilterProperty) && !string.IsNullOrEmpty(queryParameters.FilterValue))
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var property = Expression.Property(parameter, queryParameters.FilterProperty);
            var value = Expression.Constant(Convert.ChangeType(queryParameters.FilterValue, property.Type));

            Expression filterExpression;

            if (property.Type == typeof(string))
            {
                // Use Contains for string properties
                var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
                filterExpression = Expression.Call(property, containsMethod, value);
            }
            else
            {
                // Use Equals for non-string properties
                filterExpression = Expression.Equal(property, value);
            }

            AddCriteria(Expression.Lambda<Func<TEntity, bool>>(filterExpression, parameter));
        }

        if (!string.IsNullOrEmpty(queryParameters.OrderBy))
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var property = Expression.Property(parameter, queryParameters.OrderBy);
            var conversion = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);

            AddOrderBy(lambda, queryParameters.IsDescending);
        }

        ApplyPaging((queryParameters.PageNumber - 1) * queryParameters.PageSize, queryParameters.PageSize);
    }
}
