using AutoMapper;
using Domain.Shared.ApiResponse;
using Infrastructure;
using Domain.Organizations.Suppliers;
using Domain.Organizations.Suppliers.Dtos;


namespace Presentation.Controllers
{
  public class SuppliersController(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder) : BaseController<Supplier, SupplierDto>(serviceFactory, mapper, linkBuilder)
    {
    }
}
