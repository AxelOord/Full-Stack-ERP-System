using Application.ServiceLifetimes;
using Shared.Results;
using Warehouse.Application.Articles.Commands.CreateArticle;
using Warehouse.Domain.Articles;

namespace Warehouse.Persistence.Articles
{
  public sealed class CreateArticleService : ICreateArticleService, IScoped
  {
    private readonly WarehouseDbContext _context;

    public CreateArticleService(WarehouseDbContext context)
    {
      _context = context;
    }

    public async Task<Result> ExecuteAsync(Article article, CancellationToken cancellationToken)
    {
      await _context.Set<Article>().AddAsync(article, cancellationToken);

      await _context.SaveChangesAsync(cancellationToken);

      return Result.Success(article.Id);
    }
  }
}
