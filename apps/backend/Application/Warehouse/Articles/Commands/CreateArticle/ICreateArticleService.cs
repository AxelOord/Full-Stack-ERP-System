using Domain.Shared;
using Domain.Warehouse.Article;

namespace Application.Warehouse.Articles.Commands.CreateArticle
{
  public interface ICreateArticleService
  {
    Task<Result> ExecuteAsync(Article article, CancellationToken cancellationToken);
  }
}
