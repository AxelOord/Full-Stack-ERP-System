using Domain.Primitives.Interfaces;
using Shared.Specifications.Filtering;
using System.Collections;
using System.Linq.Expressions;

namespace Shared.Specifications;
public static class FilterExpressionBuilder
{
    public static Expression<Func<TEntity, bool>> BuildFilterExpression<TEntity>(FilterDefinition filterDefinition)
        where TEntity : IEntity
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");

        // Process the root group and convert it to an expression tree
        var expression = BuildGroupExpression<TEntity>(filterDefinition.RootGroup, parameter);

        // If no filters were provided, return a "true" expression
        if (expression == null)
        {
            expression = Expression.Constant(true);
        }

        return Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
    }

    private static Expression? BuildGroupExpression<TEntity>(FilterGroup group, ParameterExpression parameter)
        where TEntity : IEntity
    {
        if (group.Criteria.Count == 0 && group.Groups.Count == 0)
        {
            return null;
        }

        // Process all criteria in this group
        var criteriaExpressions = group.Criteria
            .Select(criteria => BuildCriterionExpression<TEntity>(criteria, parameter))
            .Where(expr => expr != null)
            .ToList();

        // Process all nested groups
        var groupExpressions = group.Groups
            .Select(nestedGroup => BuildGroupExpression<TEntity>(nestedGroup, parameter))
            .Where(expr => expr != null)
            .ToList();

        // Combine all expressions
        var allExpressions = criteriaExpressions.Concat(groupExpressions).ToList();

        if (allExpressions.Count == 0)
        {
            return null;
        }

        // Combine using AND or OR
        return CombineExpressions(allExpressions, group.LogicalOperator);
    }

    private static Expression? BuildCriterionExpression<TEntity>(
        FilterCriteria criteria,
        ParameterExpression parameter)
        where TEntity : IEntity
    {
        // Get property access expression
        var property = BuildPropertyExpression(parameter, criteria.Path);

        if (property == null)
        {
            return null;
        }

        // Get the property type
        var propertyType = GetPropertyType(property);

        // Convert the value to the property type
        var value = ConvertValue(criteria.Value, propertyType);
        var valueExpression = Expression.Constant(value, propertyType);

        // Apply the operator
        return ApplyOperator(property, valueExpression, criteria.Operator);
    }

    private static Expression? BuildPropertyExpression(ParameterExpression parameter, string propertyPath)
    {
        if (string.IsNullOrEmpty(propertyPath))
        {
            return null;
        }

        var parts = propertyPath.Split('.');
        Expression property = parameter;

        foreach (var part in parts)
        {
            // Get property from the current expression
            property = Expression.Property(property, part);

            if (property == null)
            {
                return null;
            }
        }

        return property;
    }

    private static Type GetPropertyType(Expression propertyExpression)
    {
        if (propertyExpression is MemberExpression memberExpression)
        {
            return memberExpression.Type;
        }

        return propertyExpression.Type;
    }

    private static object? ConvertValue(object value, Type targetType)
    {
        if (value == null)
        {
            return null;
        }

        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            targetType = Nullable.GetUnderlyingType(targetType);
        }

        if (targetType.IsEnum)
        {
            return Enum.Parse(targetType, value.ToString());
        }

        if (targetType == typeof(Guid) && value is string stringValue)
        {
            return Guid.Parse(stringValue);
        }

        if (targetType == typeof(DateTime) && value is string dateString)
        {
            return DateTime.Parse(dateString);
        }

        return Convert.ChangeType(value, targetType);
    }

    private static Expression ApplyOperator(Expression property, Expression value, string @operator)
    {
        switch (@operator.ToLower())
        {
            case FilterOperator.Equals:
                return Expression.Equal(property, value);

            case FilterOperator.NotEquals:
                return Expression.NotEqual(property, value);

            case FilterOperator.GreaterThan:
                return Expression.GreaterThan(property, value);

            case FilterOperator.GreaterThanOrEqual:
                return Expression.GreaterThanOrEqual(property, value);

            case FilterOperator.LessThan:
                return Expression.LessThan(property, value);

            case FilterOperator.LessThanOrEqual:
                return Expression.LessThanOrEqual(property, value);

            case FilterOperator.Contains:
                if (property.Type == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) }); // TODO: should be enums
                    return Expression.Call(property, containsMethod, value);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(property.Type) && property.Type != typeof(string))
                {
                    var itemType = property.Type.GetGenericArguments()[0];
                    var containsMethod = typeof(Enumerable).GetMethods()
                        .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(itemType);
                    return Expression.Call(containsMethod, property, value);
                }
                return Expression.Constant(false);

            case FilterOperator.StartsWith:
                var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                return Expression.Call(property, startsWithMethod, value);

            case FilterOperator.EndsWith:
                var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                return Expression.Call(property, endsWithMethod, value);

            case FilterOperator.IsNull:
                return Expression.Equal(property, Expression.Constant(null, property.Type));

            case FilterOperator.IsNotNull:
                return Expression.NotEqual(property, Expression.Constant(null, property.Type));

            default:
                return Expression.Equal(property, value);
        }
    }

    private static Expression? CombineExpressions(List<Expression> expressions, string logicalOperator)
    {
        if (expressions.Count == 0)
        {
            return null;
        }

        var result = expressions[0];

        for (int i = 1; i < expressions.Count; i++)
        {
            if (logicalOperator.ToUpper() == "OR") // TODO: should be enums
            {
                result = Expression.OrElse(result, expressions[i]);
            }
            else
            {
                result = Expression.AndAlso(result, expressions[i]);
            }
        }

        return result;
    }
}