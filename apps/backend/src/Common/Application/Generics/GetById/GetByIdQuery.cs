using Application.Messaging.Queries;
using Domain.Primitives.Interfaces;

namespace Application.Generics.GetById
{
  public sealed record GetByIdQuery<T>(Guid Id) : IQuery<T> where T : IEntity;
}
