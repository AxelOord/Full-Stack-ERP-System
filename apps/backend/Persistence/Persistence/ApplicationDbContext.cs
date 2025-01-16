using Domain.Organizations.Suppliers;
using Domain.Warehouse.Article;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
  public class ApplicationDbContext : DbContext, IApplicationDbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
  }
}
