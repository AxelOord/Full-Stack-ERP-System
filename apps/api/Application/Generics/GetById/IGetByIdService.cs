using Domain.Primitives;
using Domain.Shared;

namespace Application.Generics.GetById
{
    public interface IGetByIdService<T> where T : Entity
    {
        Task<Result<T>> ExecuteAsync(GetByIdQuery<T> request, CancellationToken cancellationToken);
    }
}
