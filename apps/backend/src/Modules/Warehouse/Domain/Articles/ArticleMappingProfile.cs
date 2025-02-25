using AutoMapper;
using Warehouse.Domain.Articles.Dto;

namespace Warehouse.Domain.Articles;
public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name ?? string.Empty));
        CreateMap<ArticleDto, Article>();
    }
}
