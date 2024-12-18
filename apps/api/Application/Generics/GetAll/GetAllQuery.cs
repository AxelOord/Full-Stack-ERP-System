using Application.Generics.Messaging.Queries;
using Domain.Primitives;
using Domain.Specifications;

namespace Application.Generics.GetAll
{
    public sealed record GetAllQuery<T>(ISpecification<T> Specification) : IQuery<List<T>> where T : Entity, new();
}