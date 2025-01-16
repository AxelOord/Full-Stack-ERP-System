using Application.Generics.Messaging.Queries;
using Domain.Organizations.Suppliers;
using Domain.Shared;

namespace Application.Organizations.Suppliers.GetByName
{
  public sealed class GetByNameQueryHandler(IGetByNameService getEntityByNameQuery) : IQueryHandler<GetByNameQuery, Supplier>
  {
    public async Task<Result<Supplier>> Handle(GetByNameQuery request, CancellationToken cancellationToken)
    {
      return await getEntityByNameQuery.ExecuteAsync(request, cancellationToken);
    }
  }
}
