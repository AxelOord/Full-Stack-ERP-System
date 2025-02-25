using Application.Generics.Delete;
using Application.ServiceLifetimes;
using Domain.Primitives.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Persistence.Generics.Commands;

public sealed class DeleteService<TContext, TEntity> : IDeleteService<TEntity>, IScoped
  where TContext : DbContext
  where TEntity : class, IEntity
{
    private readonly TContext _context;

    public DeleteService(TContext context)
    {
        _context = context;
    }

    public async Task<Result> ExecuteAsync(DeleteCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == request.Id);

        if (entity == null)
        {
            return Result.Failure(new NotFoundError(typeof(TEntity).Name));
        }

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
