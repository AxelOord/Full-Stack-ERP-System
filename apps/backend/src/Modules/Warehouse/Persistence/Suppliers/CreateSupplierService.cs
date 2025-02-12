using Application.ServiceLifetimes;
using Shared.Results;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Persistence.Suppliers
{
  public sealed class CreateSupplierService : ICreateSupplierService, IScoped
  {
    private readonly WarehouseDbContext _context;

    public CreateSupplierService(WarehouseDbContext context)
    {
      _context = context;
    }

    public async Task<Result> ExecuteAsync(Supplier supplier, CancellationToken cancellationToken)
    {
      await _context.Set<Supplier>().AddAsync(supplier, cancellationToken);

      await _context.SaveChangesAsync(cancellationToken);

      return Result.Success(supplier.Id);
    }
  }
}
