using Domain.Primitives.Interfaces;
using Shared.Results;

namespace Application.Generics.GetAll
{
  public interface IGetAllService<T> where T : IEntity
  {
    Task<Result<List<T>>> ExecuteAsync(GetAllQuery<T> query, CancellationToken cancellationToken);
  }
}
