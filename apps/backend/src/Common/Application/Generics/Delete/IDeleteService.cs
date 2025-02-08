using Domain.Primitives.Interfaces;
using Shared.Results;

namespace Application.Generics.Delete
{
  public interface IDeleteService<TEntity>
       where TEntity : IEntity
  {
    Task<Result> ExecuteAsync(DeleteCommand<TEntity> request, CancellationToken cancellationToken);
  }
}
