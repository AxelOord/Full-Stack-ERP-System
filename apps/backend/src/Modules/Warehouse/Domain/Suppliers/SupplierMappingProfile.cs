using AutoMapper;
using Warehouse.Domain.Suppliers.Dto;

namespace Warehouse.Domain.Suppliers;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierDto, Supplier>();
    }
}
