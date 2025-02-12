using Application.Messaging.Commands;
using Warehouse.Domain.Suppliers.Dtos;

namespace Warehouse.Application.Suppliers.Commands.CreateSupplier
{
  public sealed record CreateSupplierCommand(CreateSupplierDto Dto) : ICommand;
}
