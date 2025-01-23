using AutoMapper;
using Domain.Shared.ApiResponse;
using Infrastructure;
using Domain.Organizations.Suppliers;
using Domain.Organizations.Suppliers.Dtos;
using MediatR;


namespace Presentation.Controllers
{
  public class SuppliersController(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder, IMediator mediator)
    : BaseController<Supplier, SupplierDto>(serviceFactory, mapper, linkBuilder, mediator)
    {
    }
}
