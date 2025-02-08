using Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Shared.Results;
using Warehouse.Application.Suppliers.Queries.GetByName;
using Warehouse.Domain.Suppliers;


namespace Warehouse.Persistence.Suppliers
{
  public sealed class GetByNameService(WarehouseDbContext context) : IGetByNameService, IScoped
  {
    public async Task<Result<Supplier>> ExecuteAsync(GetByNameQuery request, CancellationToken cancellationToken)
    {
      return await context.Set<Supplier>().FirstOrDefaultAsync(s => s.Name == request.name, cancellationToken);
    }
  }
}
