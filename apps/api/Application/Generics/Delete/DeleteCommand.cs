using Application.Generics.Messaging.Commands;
using Domain.Primitives;

namespace Application.Generics.Delete
{
    public sealed record DeleteCommand<TEntity>(Guid Id) : ICommand where TEntity : Entity, new();
}
