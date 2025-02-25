using Application.Generics.GetAll;
using Application.ServiceLifetimes;
using Domain.Primitives.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Persistence.Generics.Queries;

public sealed class GetAllService<TContext, TEntity>(TContext context) : IGetAllService<TEntity>, IScoped
where TContext : DbContext
where TEntity : class, IEntity
{
    public async Task<Result<List<TEntity>>> ExecuteAsync(GetAllQuery<TEntity> query, CancellationToken cancellationToken)
    {
        var spec = query.Specification;

        IQueryable<TEntity> queryable = context.Set<TEntity>();

        // Apply filtering
        if (spec.Criteria != null)
            queryable = queryable.Where(spec.Criteria);

        queryable = IncludeAllNavigations(queryable);

        // Apply includes
        if (spec.Includes != null)
            queryable = spec.Includes.Aggregate(queryable, (current, include) => current.Include(include));

        // Apply sorting
        if (spec.OrderBy != null && spec.OrderBy.Count != 0)
        {
            var firstOrder = spec.OrderBy[0];
            IOrderedQueryable<TEntity> orderedQuery = firstOrder.IsDescending
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

    // Helper method to include all navigational properties
    private IQueryable<TEntity> IncludeAllNavigations(IQueryable<TEntity> queryable)
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));
        if (entityType == null) return queryable;

        // Get all navigation properties
        var navigations = entityType.GetNavigations();
        foreach (var navigation in navigations)
        {
            queryable = queryable.Include(navigation.Name);
        }

        return queryable;
    }
}
