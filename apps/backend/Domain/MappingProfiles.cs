using AutoMapper;
using Domain.Organizations.Suppliers;
using Domain.Organizations.Suppliers.Dtos;
using Domain.Warehouse.Article;
using Domain.Warehouse.Article.Dto;

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