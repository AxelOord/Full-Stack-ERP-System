using Application.Messaging.Queries;
using Domain.Primitives.Interfaces;
using Domain.Specifications;

namespace Application.Generics.GetAll
{
  public sealed record GetAllQuery<T>(ISpecification<T> Specification) : IQuery<List<T>> where T : IEntity;
}
