using Application.Messaging.Commands;
using Warehouse.Domain.Suppliers.Request;

namespace Warehouse.Application.Suppliers.Commands.CreateSupplier;

public sealed record CreateSupplierCommand(CreateSupplierRequest Request) : ICommand;
