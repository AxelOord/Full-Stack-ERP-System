using Domain.Organizations.Suppliers;
namespace Application.Warehouse.Suppliers
{
  public interface ISupplierRepository
  {
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  }
}
