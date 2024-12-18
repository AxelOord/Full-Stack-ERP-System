using Domain.Primitives;
using Domain.Shared;

namespace Application.Generics.Create
{
    public interface ICreateService<TDto, TEntity>
        where TDto : Dto
        where TEntity : Entity, new()
    {
        Task<Result> ExecuteAsync(CreateCommand<TDto, TEntity> request, CancellationToken cancellationToken);
    }
}
