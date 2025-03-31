using Warehouse.Domain.Articles;

namespace Warehouse.Application.Articles;

public interface IArticleRepository
{
    Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
