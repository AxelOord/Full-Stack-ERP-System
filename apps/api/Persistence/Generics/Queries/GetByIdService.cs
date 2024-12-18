using Application.Generics.GetById;
using Domain.Primitives;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Generics.Queries
{
    public sealed class GetByIdService<T>(ApplicationDbContext context) : IGetByIdService<T> 
        where T : Entity
    {
        public async Task<Result<T>> ExecuteAsync(GetByIdQuery<T> request, CancellationToken cancellationToken)
        {
            return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        }
    }
}
