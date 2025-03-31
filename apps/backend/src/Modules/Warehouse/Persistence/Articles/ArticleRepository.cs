using Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Articles;
using Warehouse.Domain.Articles;

namespace Warehouse.Persistence.Articles;

public class ArticleRepository : IArticleRepository, IScoped
{
    private readonly WarehouseDbContext _context;

    public ArticleRepository(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Article>()
            .Include(a => a.Variants)
            .Include(a => a.Supplier)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}
