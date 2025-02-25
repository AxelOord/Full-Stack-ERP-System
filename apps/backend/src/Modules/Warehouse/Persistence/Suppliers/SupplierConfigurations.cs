using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Persistence.Suppliers;

public class SupplierConfigurations : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SbnId).IsRequired();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.IsActive).IsRequired();

        builder.HasIndex(s => s.SbnId)
          .IsUnique();

        builder.HasMany(s => s.Articles)
          .WithOne(a => a.Supplier)
          .HasForeignKey(a => a.SupplierId)
          .OnDelete(DeleteBehavior.Restrict); // Enforce referential integrity
    }
}
