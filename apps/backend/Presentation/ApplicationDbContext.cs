using Domain.Articles;
using Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace Persistence
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
