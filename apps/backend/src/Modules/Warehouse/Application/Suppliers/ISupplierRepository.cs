using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers
{
  public interface ISupplierRepository
  {
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  }
}
