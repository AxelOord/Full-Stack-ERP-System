using AutoMapper;
using Warehouse.Domain.Articles;
using Warehouse.Domain.Articles.Dtos;
using Warehouse.Domain.Suppliers;
using Warehouse.Domain.Suppliers.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDto>()
          .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name ?? string.Empty));
        CreateMap<ArticleDto, Article>();

        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierDto, Supplier>();
    }
}
