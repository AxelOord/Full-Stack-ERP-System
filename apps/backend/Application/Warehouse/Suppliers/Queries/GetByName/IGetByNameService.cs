using Application.Generics.GetById;
using Domain.Organizations.Suppliers;
using Domain.Primitives;
using Domain.Shared;

namespace Application.Warehouse.Suppliers.Queries.GetByName
{
  public interface IGetByNameService
  {
    Task<Result<Supplier>> ExecuteAsync(GetByNameQuery request, CancellationToken cancellationToken);
  }
}
