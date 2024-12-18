using Application.Generics.Messaging.Commands;
using Domain.Primitives;

namespace Application.Generics.Create
{
    public sealed record CreateCommand<TDto, TEntity>(TDto Dto) : ICommand where TDto : Dto where TEntity : Entity, new();
}
