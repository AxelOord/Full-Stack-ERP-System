using AutoMapper;
using Domain.Shared.ApiResponse;
using Infrastructure;
using Domain.Warehouse.Article;
using Domain.Warehouse.Article.Dto;

namespace Presentation.Controllers
{
  public class ArticlesController(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder) : BaseController<Article, ArticleDto>(serviceFactory, mapper, linkBuilder)
    {
    }
}
