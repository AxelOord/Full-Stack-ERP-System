using Application.Generics.Messaging.Queries;
using Domain.Primitives.Interfaces;
using Domain.Shared;

namespace Application.Generics.GetById
{
    public sealed class GetByIdQueryHandler<T>(IGetByIdService<T> getEntityByIdQuery) : IQueryHandler<GetByIdQuery<T>, T>
        where T : IEntity
    {
        public async Task<Result<T>> Handle(GetByIdQuery<T> request, CancellationToken cancellationToken)
        {
            return await getEntityByIdQuery.ExecuteAsync(request, cancellationToken);
        }
    }
}
