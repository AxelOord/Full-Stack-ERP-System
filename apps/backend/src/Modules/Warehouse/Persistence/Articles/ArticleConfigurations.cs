using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Articles;

namespace Warehouse.Persistence.Articles;

public class ArticleConfigurations : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ArticleNumber).IsRequired();
        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.IsActive).IsRequired();

        builder.HasOne(a => a.Supplier)
            .WithMany()
            .HasForeignKey(a => a.SupplierId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
