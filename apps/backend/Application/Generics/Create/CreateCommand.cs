using Application.Generics.Messaging.Commands;
using Domain.Primitives;
using Domain.Primitives.Interfaces;

namespace Application.Generics.Create
{
  public sealed record CreateCommand<TDto, TEntity>(TDto Dto) : ICommand where TDto : IDto where TEntity : class, IEntity, ICreatableFromDto<TEntity, TDto>;
}
