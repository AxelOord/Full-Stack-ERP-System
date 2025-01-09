using Domain.Primitives;
using Domain.Shared;

namespace Application.Generics.Delete
{
    public interface IDeleteService<TEntity>
       where TEntity : Entity, new()
    {
        Task<Result> ExecuteAsync(DeleteCommand<TEntity> request, CancellationToken cancellationToken);
    }
}
