using Domain.Organizations.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Suppliers
{
  public class SupplierConfigurations : IEntityTypeConfiguration<Supplier>
  {
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SbnId).IsRequired();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.IsActive).IsRequired();
    }
  }
}
