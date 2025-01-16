using Domain.Primitives.Interfaces;
using System.Linq.Expressions;
namespace Domain.Specifications
{
    public class GetAllEntitiesSpecification<TEntity> : Specification<TEntity> where TEntity : IEntity
    {
        public GetAllEntitiesSpecification(
            GetAllQueryParameters queryParameters)
        {
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


            //if (orderBy != null)
            //    foreach (var order in orderBy)
            //        AddOrderBy(order.KeySelector, order.IsDescending);

            //if (includes != null)
            //    foreach (var include in includes)
            //        AddInclude(include);

            ApplyPaging((queryParameters.PageNumber - 1) * queryParameters.PageSize, queryParameters.PageSize);
        }
    }
}
