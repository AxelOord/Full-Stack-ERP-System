using Shared.Results;
using Warehouse.Domain.Articles;

namespace Warehouse.Application.Articles.Commands.CreateArticle;

public interface ICreateArticleService
{
    Task<Result> ExecuteAsync(Article article, CancellationToken cancellationToken);
}
