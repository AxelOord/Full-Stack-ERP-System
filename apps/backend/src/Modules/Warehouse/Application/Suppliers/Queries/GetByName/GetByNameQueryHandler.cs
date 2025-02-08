using Application.Messaging.Queries;
using Shared.Results;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries.GetByName
{
  public sealed class GetByNameQueryHandler(IGetByNameService getEntityByNameQuery) : IQueryHandler<GetByNameQuery, Supplier>
  {
    public async Task<Result<Supplier>> Handle(GetByNameQuery request, CancellationToken cancellationToken)
    {
      return await getEntityByNameQuery.ExecuteAsync(request, cancellationToken);
    }
  }
}
