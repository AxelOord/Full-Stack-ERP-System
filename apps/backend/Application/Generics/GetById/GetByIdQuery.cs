using Application.Generics.Messaging.Queries;
using Domain.Primitives;
using MediatR;

namespace Application.Generics.GetById
{
    public sealed record GetByIdQuery<T>(Guid Id) : IQuery<T> where T : Entity;
}
