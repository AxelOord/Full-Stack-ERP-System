using Application.ServiceLifetimes;
using Shared.Results;
using Warehouse.Application.Articles.Commands.CreateArticleVariant;
using Warehouse.Domain.Articles;

namespace Warehouse.Persistence.Articles;

public sealed class CreateArticleVariantService : ICreateArticleVariantService, IScoped
{
    private readonly WarehouseDbContext _context;

    public CreateArticleVariantService(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<Result> ExecuteAsync(ArticleVariant variant, CancellationToken cancellationToken)
    {
        await _context.Set<ArticleVariant>().AddAsync(variant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(variant.Id);
    }
}
