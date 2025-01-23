
using Application.Warehouse.Articles.Commands.CreateArticle;
using Domain.Shared;
using Domain.Warehouse.Article;

namespace Infrastructure.Persistence.Articles
{
  public sealed class CreateArticleService : ICreateArticleService
  {
    private readonly ApplicationDbContext _context;

    public CreateArticleService(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Result> ExecuteAsync(Article article, CancellationToken cancellationToken)
    {
      await _context.Articles.AddAsync(article, cancellationToken);

      await _context.SaveChangesAsync(cancellationToken);

      return Result.Success(article.Id);
    }
  }
}
