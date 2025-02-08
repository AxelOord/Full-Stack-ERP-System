using Domain.Primitives.Interfaces;
using Shared.Results;

namespace Application.Generics.GetById
{
  public interface IGetByIdService<T> where T : IEntity
  {
    Task<Result<T>> ExecuteAsync(GetByIdQuery<T> request, CancellationToken cancellationToken);
  }
}
