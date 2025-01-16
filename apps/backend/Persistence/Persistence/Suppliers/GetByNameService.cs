using Application.Generics.GetById;
using Application.Organizations.Suppliers.GetByName;
using Domain.Organizations.Suppliers;
using Domain.Shared;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Persistence.Suppliers
{
  public sealed class GetByNameService(ApplicationDbContext context) : IGetByNameService
  {
    public async Task<Result<Supplier>> ExecuteAsync(GetByNameQuery request, CancellationToken cancellationToken)
    {
      return await context.Set<Supplier>().FirstOrDefaultAsync(s => s.Name == request.name, cancellationToken);
    }
  }
}
