using Domain.Primitives;
using Domain.Shared;

namespace Application.Generics.GetAll
{
    public interface IGetAllService<T> where T : Entity, new()
    {
        Task<Result<List<T>>> ExecuteAsync(GetAllQuery<T> query, CancellationToken cancellationToken);
    }
}
