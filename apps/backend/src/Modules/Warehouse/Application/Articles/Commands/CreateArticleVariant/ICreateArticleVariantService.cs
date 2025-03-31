using Shared.Results;
using Warehouse.Domain.Articles;

namespace Warehouse.Application.Articles.Commands.CreateArticleVariant;

public interface ICreateArticleVariantService
{
    Task<Result> ExecuteAsync(ArticleVariant variant, CancellationToken cancellationToken);
}
