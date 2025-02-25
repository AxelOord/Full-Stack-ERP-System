using Application.Generics.GetById;
using Application.ServiceLifetimes;
using Domain.Primitives.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Persistence.Generics.Queries;

public sealed class GetByIdService<TContext, TEntity>(TContext context) : IGetByIdService<TEntity>, IScoped
  where TContext : DbContext
  where TEntity : class, IEntity
{
    public async Task<Result<TEntity>> ExecuteAsync(GetByIdQuery<TEntity> request, CancellationToken cancellationToken)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
    }
}
