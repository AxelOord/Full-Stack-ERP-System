using Application.Generics.Messaging.Queries;
using Domain.Primitives.Interfaces;
using Domain.Shared;

namespace Application.Generics.GetAll
{
    public sealed class GetAllQueryHandler<TEntity>(IGetAllService<TEntity> getEntitiesQuery) : IQueryHandler<GetAllQuery<TEntity>, List<TEntity>>
        where TEntity : IEntity, new()
    {
        public async Task<Result<List<TEntity>>> Handle(GetAllQuery<TEntity> request, CancellationToken cancellationToken)
        {
            return await getEntitiesQuery.ExecuteAsync(request, cancellationToken);
        }
    }
}
