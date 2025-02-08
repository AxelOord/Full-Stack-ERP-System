using Domain.Primitives.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specifications
{
  public abstract class Specification<TEntity> : ISpecification<TEntity>
        where TEntity : IEntity
  {
    public Expression<Func<TEntity, bool>> Criteria { get; protected set; } = x => true;
    public List<SortOrder<TEntity>> OrderBy { get; } = [];
    public int Skip { get; protected set; } = 0;
    public int Take { get; protected set; } = int.MaxValue;
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

    protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
    {
      Criteria = criteria;
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> keySelector, bool isDescending = false)
    {
      OrderBy.Add(new SortOrder<TEntity> { KeySelector = keySelector, IsDescending = isDescending });
    }

    protected void ApplyPaging(int skip, int take)
    {
      Skip = skip;
      Take = take;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
      Includes.Add(includeExpression);
    }
  }
}
