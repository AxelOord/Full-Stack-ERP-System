using Shared.Results;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries.GetByName;

public interface IGetByNameService
{
    Task<Result<Supplier>> ExecuteAsync(GetByNameQuery request, CancellationToken cancellationToken);
}
