using Application.Generics.Delete;
using Domain.Primitives;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Generics.Commands
{
    public sealed class DeleteService<TEntity> : IDeleteService<TEntity>
        where TEntity : Entity, new()
    {
        private readonly ApplicationDbContext _context;

        public DeleteService(ApplicationDbContext context)
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
}
