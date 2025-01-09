using AutoMapper;
using Api.Infrastructure;
using Domain.Shared.ApiResponse;
using Domain.Organizations;


namespace Api.Controllers
{
    public class SuppliersController(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder) : BaseController<Supplier, SupplierDto>(serviceFactory, mapper, linkBuilder)
    {
    }
}
