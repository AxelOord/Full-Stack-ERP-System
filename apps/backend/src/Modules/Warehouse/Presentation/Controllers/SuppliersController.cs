
using AutoMapper;
using MediatR;
using Presentation;
using Shared.Results.Response;
using Warehouse.Domain.Suppliers;
using Warehouse.Domain.Suppliers.Dtos;
using Warehouse.Persistence;

namespace Warehouse.Presentation.Controllers
{
  public class SuppliersController(IMapper mapper, LinkBuilder linkBuilder, IMediator mediator, Func<Type, Type, Type, object> factory)
    : BaseController<Supplier, SupplierDto, WarehouseDbContext>(mapper, linkBuilder, mediator, factory)
  {
  }
}
