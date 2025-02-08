using Domain.Primitives.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specifications
{
  public interface ISpecification<TEntity>
        where TEntity : IEntity
  {
    Expression<Func<TEntity, bool>> Criteria { get; }

    List<SortOrder<TEntity>> OrderBy { get; }

    int Skip { get; }
    int Take { get; }

    List<Expression<Func<TEntity, object>>> Includes { get; }
  }
}
