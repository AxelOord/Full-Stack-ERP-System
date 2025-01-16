using Application.Generics.Messaging.Queries;
using Domain.Organizations.Suppliers;
using Domain.Primitives;

namespace Application.Organizations.Suppliers.GetByName
{
  public sealed record GetByNameQuery(string name) : IQuery<Supplier>;
}
