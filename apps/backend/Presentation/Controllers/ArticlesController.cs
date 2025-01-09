using Domain.Articles;
using AutoMapper;
using Api.Infrastructure;
using Domain.Shared.ApiResponse;

namespace Api.Controllers
{
    public class ArticlesController(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder) : BaseController<Article, ArticleDto>(serviceFactory, mapper, linkBuilder)
    {
    }
}
