using Shared.Results;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands.CreateSupplier;

public interface ICreateSupplierService
{
    Task<Result> ExecuteAsync(Supplier article, CancellationToken cancellationToken);
}
