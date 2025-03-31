using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Articles;

namespace Warehouse.Persistence.Articles;

public class ArticleVariantConfiguration : IEntityTypeConfiguration<ArticleVariant>
{
    public void Configure(EntityTypeBuilder<ArticleVariant> builder)
    {
        builder.HasKey(av => av.Id);

        builder.Property(av => av.Color).IsRequired();
        builder.Property(av => av.Width).IsRequired();
        builder.Property(av => av.IsActive).IsRequired();

        builder.HasOne(av => av.Article)
            .WithMany(a => a.Variants)
            .HasForeignKey(av => av.ArticleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
