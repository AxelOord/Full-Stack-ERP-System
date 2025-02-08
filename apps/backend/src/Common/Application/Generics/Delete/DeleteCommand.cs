using Application.Messaging.Commands;
using Domain.Primitives.Interfaces;

namespace Application.Generics.Delete
{
  public sealed record DeleteCommand<TEntity>(Guid Id) : ICommand where TEntity : IEntity;
}
