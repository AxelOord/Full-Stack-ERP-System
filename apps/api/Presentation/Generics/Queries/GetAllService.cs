using Application.Generics.GetAll;
using Domain.Primitives;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Generics.Queries
{
    public sealed class GetAllService<T>(ApplicationDbContext context) : IGetAllService<T> where T : Entity, new()
    {
        public async Task<Result<List<T>>> ExecuteAsync(GetAllQuery<T> query, CancellationToken cancellationToken)
        {
            var spec = query.Specification;

            IQueryable<T> queryable = context.Set<T>();

            // Apply filtering
            if (spec.Criteria != null)
                queryable = queryable.Where(spec.Criteria);

            // Apply includes
            if (spec.Includes != null)
                queryable = spec.Includes.Aggregate(queryable, (current, include) => current.Include(include));

            // Apply sorting
            if (spec.OrderBy != null && spec.OrderBy.Count != 0)
            {
                var firstOrder = spec.OrderBy[0];
                IOrderedQueryable<T> orderedQuery = firstOrder.IsDescending
                    ? queryable.OrderByDescending(firstOrder.KeySelector)
                    : queryable.OrderBy(firstOrder.KeySelector);

                foreach (var order in spec.OrderBy.Skip(1))
                {
                    orderedQuery = order.IsDescending
                        ? orderedQuery.ThenByDescending(order.KeySelector)
                        : orderedQuery.ThenBy(order.KeySelector);
                }

                queryable = orderedQuery;
            }

            // Apply pagination
            queryable = queryable.Skip(spec.Skip).Take(spec.Take);

            var result = await queryable.ToListAsync(cancellationToken);
            return Result.Success(result);
        }
    }
}
