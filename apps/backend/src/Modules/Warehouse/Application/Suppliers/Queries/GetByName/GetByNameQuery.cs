using Application.Messaging.Queries;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries.GetByName
{
  public sealed record GetByNameQuery(string name) : IQuery<Supplier>;
}
