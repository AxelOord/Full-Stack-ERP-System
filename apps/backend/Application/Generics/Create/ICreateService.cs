using Domain.Primitives;
using Domain.Primitives.Interfaces;
using Domain.Shared;

namespace Application.Generics.Create
{
  public interface ICreateService<TDto, TEntity>
        where TDto : IDto
        where TEntity : class, IEntity, ICreatableFromDto<TEntity, TDto>
    {
        Task<Result> ExecuteAsync(CreateCommand<TDto, TEntity> request, CancellationToken cancellationToken);
    }
}
