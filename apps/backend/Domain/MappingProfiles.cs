using AutoMapper;
using Domain.Articles;
using Domain.Organizations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDto>();
        CreateMap<ArticleDto, Article>();

        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierDto, Supplier>();
    }
}