using Domain.Primitives;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public interface ISpecification<TEntity>
        where TEntity : Entity, new()
    {
        Expression<Func<TEntity, bool>> Criteria { get; }

        List<SortOrder<TEntity>> OrderBy { get; }

        int Skip { get; }
        int Take { get; }

        List<Expression<Func<TEntity, object>>> Includes { get; }
    }
}
