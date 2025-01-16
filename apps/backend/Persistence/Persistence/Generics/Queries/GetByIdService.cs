using Application.Generics.GetById;
using Domain.Primitives.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Generics.Queries
{
  public sealed class GetByIdService<T>(ApplicationDbContext context) : IGetByIdService<T>
      where T : class, IEntity
  {
    public async Task<Result<T>> ExecuteAsync(GetByIdQuery<T> request, CancellationToken cancellationToken)
    {
      return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
    }
  }
}
